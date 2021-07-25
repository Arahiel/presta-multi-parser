using ECommerceParser.Helpers.Enums;
using ECommerceParser.Helpers.Wallpics;
using ECommerceParser.Model.Artb2b;
using ECommerceParser.Model.Common;
using ECommerceParser.Model.Prestashop;
using ECommerceParser.Parsers.Common;
using EuropeanCentralBank.ExchangeRates;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECommerceParser.Parsers.Artb2b
{
    public class ProductParser : GenericParser<ImportedFile, ExportedProductsFile, ExportedProductVariantsFile>
    {
        private Currencies _currency;
        /// <summary>
        /// Currency to which import prices will be converted. It is output currency it the exported file.
        /// </summary>
        protected override Currencies Currency => _currency;

        public ProductParser(Currencies currency)
        {
            _currency = currency;
        }

        public override async Task<(ExportedProductsFile productFile, ExportedProductVariantsFile productVariantsFile)> Parse(ImportedFile importObject, Language fileLanguage)
        {
            var products = new List<ExportedProduct>();
            var productVariants = new List<ExportedProductVariant>();
            var lastProductId = -1;

            foreach (var importedProduct in importObject.Products)
            {
                //Features
                var splittedFeatures = importedProduct.Features.Split('|');
                var buckets = splittedFeatures.Batch(2);
                var featureDict = buckets.ToDictionary(
                x =>
                {
                    var values = x.ToArray();
                    var nameCell = values[0];
                    var name = Regex.Match(nameCell, @"name=(.*)").Groups[1].Value;
                    return name;
                },
                 x =>
                 {
                     var values = x.ToArray();
                     var valueCell = values[1];
                     var value = Regex.Match(valueCell, @"value=(.*)").Groups[1].Value;
                     return value;
                 });

                var id = int.Parse(featureDict["Źródło"]);

                /*Omit any other combinations of the same product 
                 * (TODO: Implement returning only the product with specific layout - passed in parameter)
                 */ 
                if (id == lastProductId)
                {
                    continue;
                }
                lastProductId = id;

                //Categories
                var cb = new CategoryBuilder();
                var categoryStrings = importedProduct.Categories.Split('|');
                var categories = new Categories();
                foreach (var categoryString in categoryStrings)
                {
                    var categoryNames = categoryString.Split('/');
                    foreach (var categoryName in categoryNames)
                    {
                        cb.AddCategory(categoryName);
                    }
                    categories.Add(cb.Build());
                    cb.Clear();
                }

                //Images
                var imageUrls = importedProduct.Images.Split('|').ToList();

                //Prices in Euro (default and only one for our shop)
                var exchange = new ExchangeRatesCalculator(new ExchangeRatesSource());
                var convertedPriceWithTax = await exchange.Calculate(importObject.ImportFileCurrency,
                    Currency,
                    importedProduct.Price_with_tax);
                var convertedPurchasePrice = await exchange.Calculate(importObject.ImportFileCurrency,
                    Currency,
                    importedProduct.Purchase_price);
                var finalPriceWithTax = convertedPriceWithTax + Constants.Margin * convertedPriceWithTax;

                //Tags
                var tags = importedProduct.Tags.Split('|');

                //Tax rule
                var taxRule = Constants.WallpicsTaxRules[importedProduct.Tax];

                var product = new ExportedProduct(id, importedProduct.Code, finalPriceWithTax, taxRule, convertedPurchasePrice);
                product.Name = importedProduct.Name;
                product.Description = importedProduct.Description;
                product.Categories = categories;
                product.ImageUrls = imageUrls;
                product.Tags = tags.Select(x => new Tag(x)).ToList();

                products.Add(product);
            }
            //TODO: Translate to all needed languages here?
            return (new ExportedProductsFile(products, fileLanguage), new ExportedProductVariantsFile(productVariants, fileLanguage));
        }
    }
}
