using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Parsers.Common
{
    public abstract class GenericParser<TImportFile, TProduct>
    {
        public abstract TProduct Load(TImportFile filePath);
    }
}
