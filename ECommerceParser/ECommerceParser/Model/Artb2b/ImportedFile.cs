using ECommerceParser.Helpers.Enums;
using EuropeanCentralBank.ExchangeRates;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Artb2b
{
    public class ImportedFile
    {
        public List<ImportedProduct> Products { get; }
        public Currencies ImportFileCurrency { get; }

        private ImportedFile(List<ImportedProduct> products, Currencies importFileCurrency)
        {
            Products = products;
            ImportFileCurrency = importFileCurrency;
        }

        public static ImportedFile Load(string filePath, Currencies importFileCurrency) =>
            Load(File.ReadAllLines(filePath).Skip(1), importFileCurrency);

        public static ImportedFile Load(IEnumerable<string> contentLines, Currencies importFileCurrency)
        {
            var products = new List<ImportedProduct>();

            foreach (var line in contentLines.Skip(1))
            {
                if (line.Equals(string.Empty))
                {
                    continue;
                }

                var regex = new Regex("(\".*?\")");
                var sanitizedLine = regex.Replace(line, x => x.Value.Replace(';', '|'));
                var values = sanitizedLine.Split(';').Select(x => x.Trim('\"')).ToArray();

                products.Add(new ImportedProduct(
                    int.Parse(values[(int)ImportHeaders.Id]),
                    values[(int)ImportHeaders.Code],
                    values[(int)ImportHeaders.Name],
                    values[(int)ImportHeaders.Description],
                    double.Parse(values[(int)ImportHeaders.Price_with_tax], CultureInfo.InvariantCulture),
                    int.Parse(values[(int)ImportHeaders.Tax], CultureInfo.InvariantCulture),
                    double.Parse(values[(int)ImportHeaders.Purchase_price], CultureInfo.InvariantCulture),
                    values[(int)ImportHeaders.Categories],
                    values[(int)ImportHeaders.Variants],
                    values[(int)ImportHeaders.Features],
                    values[(int)ImportHeaders.Images],
                    values[(int)ImportHeaders.Tags]
                    ));
            }

            return new ImportedFile(products, importFileCurrency);
        }
    }
}
