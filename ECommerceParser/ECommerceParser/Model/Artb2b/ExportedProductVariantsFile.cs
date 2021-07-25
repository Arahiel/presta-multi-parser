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
        public Language FileLanguage;

        public ExportedProductVariantsFile(List<ExportedProductVariant> productVariants, Language fileLanguage)
        {
            ProductVariants = productVariants;
            FileLanguage = fileLanguage;
        }

        public string ToCsv()
        {
            throw new NotImplementedException();
        }
    }
}
