using ECommerceParser.Model.Artb2b;
using ECommerceParser.Model.Prestashop;
using ECommerceParser.Parsers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Parsers.Artb2b
{
    public class CsvParser : GenericParser<ImportFile, Product>
    {
        public override Product Load(ImportFile filePath)
        {
            throw new NotImplementedException();
        }
    }
}
