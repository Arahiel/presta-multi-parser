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
    public class ExportedProductsFile : ICsvSerializable
    {
        public List<ExportedProduct> Products;
        public Language FileLanguage;

        public ExportedProductsFile(List<ExportedProduct> products, Language fileLanguage)
        {
            Products = products;
            FileLanguage = fileLanguage;
        }

        public string ToCsv()
        {
            throw new NotImplementedException();
        }
    }
}
