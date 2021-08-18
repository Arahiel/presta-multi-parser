using ECommerceParser.Helpers.Enums;
using ECommerceParser.Helpers.Enums.Prestashop;
using ECommerceParser.Helpers.Wallpics;
using ECommerceParser.Model.Artb2b;
using ECommerceParser.Model.Common;
using ECommerceParser.Model.Prestashop;
using ECommerceParser.Parsers.Common;
using EuropeanCentralBank.ExchangeRates;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECommerceParser.Parsers.Artb2b
{
    public class ProductParser : GenericParser<ImportedFile, ExportedProductsFile, ExportedProductVariantsFile>, INotifyPropertyChanged
    {
        private int _currentParsedProductIndex;
        private Currencies _currency;

        public int CurrentParsedProductIndex
        {
            get => _currentParsedProductIndex;
            set
            {
                _currentParsedProductIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentParsedProductIndex)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Currency to which import prices will be converted. It is output currency in the exported file.
        /// </summary>
        protected override Currencies Currency => _currency;

        /// <summary>
        /// Parameter is the currency to which import prices will be converted. It is output currency in the exported file.
        /// </summary>
        /// <param name="currency"></param>
        public ProductParser(Currencies currency)
        {
            _currency = currency;
            CurrentParsedProductIndex = 0;
        }

        public override async Task<(ExportedProductsFile productFile, ExportedProductVariantsFile productVariantsFile)> ParseProducts(ImportedFile importObject, string sourceLanguageCode)
        {
            CurrentParsedProductIndex = 0;

            var products = await GetExportedProducts(importObject);
            var productsWithVariants = GetExportedProductVariants(products.OrderBy(x => x.Id));

            return (new ExportedProductsFile(productsWithVariants, sourceLanguageCode),
                new ExportedProductVariantsFile(productsWithVariants.SelectMany(x => x.Variants).ToList(), sourceLanguageCode));
        }

        private List<ExportedProduct> GetExportedProductVariants(IEnumerable<ExportedProduct> products)
        {
            var productVariants = new List<ExportedProductVariant>();
            var lastId = -1;
            var defaultPrice = 0d;

            foreach (var product in products)
            {
                var isDefault = product.Id != lastId;
                double impactOnPrice;
                if (isDefault)
                {
                    defaultPrice = product.PriceTaxIncluded;
                    impactOnPrice = 0;
                }
                else
                {
                    impactOnPrice = product.PriceTaxIncluded - defaultPrice;
                }

                const string inputLayoutAttributeName = "Układ";
                const string outputLayoutAttributeName = "Layout";
                const uint layoutPosition = 0;

                var attributes = new Attributes()
                {
                    new Model.Prestashop.Attribute(outputLayoutAttributeName, AttributeType.Select, layoutPosition, new AttributeValue(layoutPosition, product.Features[inputLayoutAttributeName]))
                };

                const int quantity = 0; // Add empty stock by default
                productVariants.Add(new ExportedProductVariant(product.Id, attributes, product.Reference, impactOnPrice, quantity, isDefault, product.ImageUrls));
                lastId = product.Id;
            }

            var outProducts = products.Where(x => x.Reference.Equals(productVariants.Single(y => y.Id == x.Id && y.Default).Reference));
            outProducts.ForEach(x => x.Variants = productVariants.Where(y => y.Id == x.Id).ToList());
            return outProducts.ToList();
        }

        private async Task<List<ExportedProduct>> GetExportedProducts(ImportedFile importObject)
        {
            var products = new List<ExportedProduct>();

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
                var imageUrls = new ImageUrls(importedProduct.Images.Split('|'));

                //Prices in Euro (default and only one for our shop)
                var exchange = new ExchangeRatesCalculator(new ExchangeRatesSource());
                var convertedPriceWithTax = await exchange.Calculate(importObject.ImportFileCurrency,
                    Currency,
                    importedProduct.Price_with_tax);
                var convertedPurchasePrice = await exchange.Calculate(importObject.ImportFileCurrency,
                    Currency,
                    importedProduct.Purchase_price);
                var finalPriceWithTax = Math.Round(convertedPriceWithTax + Constants.Margin * convertedPriceWithTax); //Rounded price

                //Tags
                var tags = importedProduct.Tags.Split('|');

                //Tax rule
                var taxRule = Constants.WallpicsTaxRules[importedProduct.Tax];

                var product = new ExportedProduct(id, importedProduct.Code, finalPriceWithTax, taxRule, convertedPurchasePrice, featureDict);
                product.Name = importedProduct.Name;
                product.Description = importedProduct.Description;
                product.Categories = categories;
                product.ImageUrls = imageUrls;
                product.Tags = Tags.FromListWithUpdatedHandler(tags.Select(x => new Tag(x)).ToList());

                products.Add(product);
                CurrentParsedProductIndex++;
            }

            return products;
        }
    }
}
