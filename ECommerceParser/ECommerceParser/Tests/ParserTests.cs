using ECommerceParser.Helpers;
using ECommerceParser.Model.Artb2b;
using ECommerceParser.Parsers.Artb2b;
using ECommerceParser.Properties;
using Google.Cloud.Translation.V2;
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
            var exportedProductFileContents = Resources.artb2b_20210717_103921_products_en;
            var exportedProductVariantsFileContents = Resources.artb2b_20210717_103921_combinations_en;

            var parser = new ProductParser(EuropeanCentralBank.ExchangeRates.Currencies.Euro, 1);
            var (outputProductFile, outputProductVariantsFile) = await parser.ParseProducts(importedFile, LanguageCodes.Polish);

            var translator = new Translator();
            var translatedProductFile = await translator.Translate(outputProductFile, LanguageCodes.English);

            string productFileCsvString = translatedProductFile.ToCsv();
            string productVariantsFileCsvString = outputProductVariantsFile.ToCsv();

            Assert.That(productFileCsvString, Is.EqualTo(exportedProductFileContents));
            Assert.That(productVariantsFileCsvString, Is.EqualTo(exportedProductVariantsFileContents));
        }
    }
}
