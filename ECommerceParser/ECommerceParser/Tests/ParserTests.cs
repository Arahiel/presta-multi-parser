using ECommerceParser.Model.Artb2b;
using ECommerceParser.Parsers.Artb2b;
using ECommerceParser.Properties;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Tests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public async Task Artb2bCsvParserCreatesProperFile()
        {
            var importedFile = ImportedFile.Load(Resources.artb2b_20210717_103921.Split('\n'), 
                EuropeanCentralBank.ExchangeRates.Currencies.PolishZloty);
            var exportedProductFileContents = Resources.artb2b_20210717_103921_products;
            var exportedProductVariantsFileContents = Resources.artb2b_20210717_103921_combinations;

            var parser = new ProductParser(EuropeanCentralBank.ExchangeRates.Currencies.Euro);
            var (outputProductFileContents, outputProductVariantsFileContents) = await parser.Parse(importedFile, Helpers.Enums.Language.Polish);

            Assert.That(outputProductFileContents.ToCsv(), Is.EqualTo(exportedProductFileContents));
            //Assert.That(outputProductVariantsFileContents, Is.EqualTo(exportedProductVariantsFileContents));
        }
    }
}
