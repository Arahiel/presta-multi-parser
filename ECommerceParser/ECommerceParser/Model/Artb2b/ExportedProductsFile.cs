using ECommerceParser.Helpers.Enums;
using ECommerceParser.Interfaces;
using ECommerceParser.Model.Prestashop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Artb2b
{
    public class ExportedProductsFile : ICsvSerializable
    {
        public List<ExportedProduct> Products;
        public string FileLanguageCode;

        public ExportedProductsFile(List<ExportedProduct> products, string fileLanguageCode)
        {
            Products = products;
            FileLanguageCode = fileLanguageCode;
        }

        public string ToCsv()
        {
            var sb = new StringBuilder();

            AppendHeaders(sb);
            AppendProducts(sb);

            return sb.ToString();
        }

        private void AppendHeaders(StringBuilder sb)
        {
            sb.Append("Id;Reference;Name;Description;Price tax included;Tax rule id;Cost price;Categories;Images;Tags\r\n");
        }

        private void AppendProducts(StringBuilder sb)
        {
            foreach (var product in Products)
            {
                sb.Append($"{product.Id};" +
                    $"{product.Reference};" +
                    $"{product.Name};" +
                    $"{product.Description};" +
                    $"{product.PriceTaxIncluded};" +
                    $"{product.TaxRule};" +
                    $"{product.CostPrice};" +
                    $"{product.Categories};" +
                    $"{string.Join(",", product.ImageUrls)};" +
                    $"{product.Tags}\r\n");
            }
        }
    }
}
