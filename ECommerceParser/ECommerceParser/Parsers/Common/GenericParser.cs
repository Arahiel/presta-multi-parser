using ECommerceParser.Helpers.Enums;
using EuropeanCentralBank.ExchangeRates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Parsers.Common
{
    public abstract class GenericParser<TImportFile, TProductFile, TProductVariantsFile>
    {
        protected abstract Currencies Currency { get; }
        public abstract Task<(TProductFile productFile, TProductVariantsFile productVariantsFile)> ParseProducts(TImportFile importObject, Language fileLanguage);
    }
}
