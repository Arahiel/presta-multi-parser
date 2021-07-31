using ECommerceParser.Helpers.Enums;
using ECommerceParser.Interfaces;
using ECommerceParser.Model.Prestashop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Artb2b
{
    public class ExportedProductVariantsFile : ICsvSerializable
    {
        public List<ExportedProductVariant> ProductVariants;
        public string FileLanguageCode;

        public ExportedProductVariantsFile(List<ExportedProductVariant> productVariants, string fileLanguageCode)
        {
            ProductVariants = productVariants;
            FileLanguageCode = fileLanguageCode;
        }

        public string ToCsv()
        {
            var sb = new StringBuilder();

            AppendHeaders(sb);
            AppendProductVariants(sb);

            return sb.ToString();
        }

        private void AppendHeaders(StringBuilder sb)
        {
            sb.Append("Product ID;Attributes;Values;Reference;Impact on price;Quantity;Default;Image URLs (x,y,z...)\r\n");
        }
        private void AppendProductVariants(StringBuilder sb)
        {
            foreach (var productVariant in ProductVariants)
            {
                sb.Append($"{productVariant.Id};" +
                    $"{productVariant.Attributes};" +
                    $"{productVariant.Attributes.Values()};" +
                    $"{productVariant.Reference};" +
                    $"{productVariant.ImpactOnPrice};" +
                    $"{productVariant.Quantity};" +
                    $"{(productVariant.Default ? "1" : "0")};" +
                    $"{string.Join(",", productVariant.ImageUrls)}\r\n");
            }
        }
    }
}
