using ECommerceParser.Helpers.Enums;
using ECommerceParser.Helpers.Wallpics;
using ECommerceParser.Model.Artb2b;
using ECommerceParser.Model.Common;
using ECommerceParser.Model.Prestashop;
using Google.Cloud.Translation.V2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Helpers
{
    public class Translator
    {
        /// <summary>
        /// Translates whole ExportedProductsFile object with products from source language to destination language.
        /// For language codes use Google.Cloud.Translation.V2.LanguageCodes helper class.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="sourceLanguageCode"></param>
        /// <param name="destinationLanguageCode"></param>
        /// <returns></returns>
        public async Task<ExportedProductsFile> Translate(ExportedProductsFile inputFile,
                                                                 string destinationLanguageCode)
        {
            if (inputFile.FileLanguageCode == destinationLanguageCode) return inputFile;

            var client = new HttpClient();
            var outputProducts = TranslateProducts(client, inputFile.Products, inputFile.FileLanguageCode, destinationLanguageCode);
            return new ExportedProductsFile(await outputProducts.ToListAsync(), destinationLanguageCode);
        }

        private async IAsyncEnumerable<ExportedProduct> TranslateProducts(HttpClient client,
                                                                                 List<ExportedProduct> inputProducts,
                                                                                 string sourceLanguageCode,
                                                                                 string destinationLanguageCode)
        {
            foreach (var product in inputProducts)
            {
                yield return (await TranslateProduct(client, product, sourceLanguageCode, destinationLanguageCode));
            }
        }

        private async Task<ExportedProduct> TranslateProduct(HttpClient client,
                                                                    ExportedProduct inputProduct,
                                                                    string sourceLanguageCode,
                                                                    string destinationLanguageCode)
        {
            var outputProduct = new ExportedProduct(inputProduct.Id, inputProduct.Reference, inputProduct.PriceTaxIncluded, inputProduct.TaxRule, inputProduct.CostPrice, inputProduct.Features);


            outputProduct.ImageUrls = inputProduct.ImageUrls;
            outputProduct.Name = await TranslateAsync(client, inputProduct.Name, sourceLanguageCode, destinationLanguageCode, Constants.TranslatorMail);

            //Maximum byte size for translation is 500 bytes, so Description has to have <=250 characters
            outputProduct.Description = await TranslateAsync(client, inputProduct.Description, sourceLanguageCode, destinationLanguageCode, Constants.TranslatorMail);

            //Translate each category separately for better translation match
            outputProduct.Categories = new Categories(await TranslateSeparateCategories(client, inputProduct, sourceLanguageCode, destinationLanguageCode).ToListAsync());

            var translatedTags = await TranslateAsync(client, inputProduct.Tags.Select(x => x.Name), sourceLanguageCode, destinationLanguageCode, Constants.TranslatorMail).ToListAsync();
            outputProduct.Tags = new Tags(translatedTags.Select(x => new Tag(x)));

            return outputProduct;
        }

        private async IAsyncEnumerable<Category> TranslateSeparateCategories(HttpClient client, ExportedProduct inputProduct, string sourceLanguageCode, string destinationLanguageCode)
        {
            foreach (var category in inputProduct.Categories)
            {
                var translations = await TranslateAsync(client, category.ToString().Split('/'), sourceLanguageCode, destinationLanguageCode, Constants.TranslatorMail).ToListAsync();
                var translatedCategory = Category.FromString(string.Join("/", translations));
                yield return translatedCategory;
            }
        }

        public static async IAsyncEnumerable<string> TranslateAsync(HttpClient client,
                                                                     IEnumerable<string> inputTexts,
                                                                     string sourceLanguageCode,
                                                                     string destinationLanguageCode,
                                                                     string translatorMail = "")
        {
            foreach (var inputText in inputTexts)
            {
                yield return await TranslateAsync(client, inputText, sourceLanguageCode, destinationLanguageCode, translatorMail);
            }
        }

        public static async Task<string> TranslateAsync(HttpClient client,
                                                         string inputText,
                                                         string sourceLanguageCode,
                                                         string destinationLanguageCode,
                                                         string translatorMail = "")
        {
            if (inputText.Equals(string.Empty)) return string.Empty;
            if (sourceLanguageCode == destinationLanguageCode) return inputText;
            var cachedTranslation = GetCachedTranslation(inputText, sourceLanguageCode, destinationLanguageCode);
            if (cachedTranslation != null)
            {
                return cachedTranslation;
            }


            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = GetTranslationUri(inputText, sourceLanguageCode, destinationLanguageCode, translatorMail)
            };

            string translatedText;

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var myMemoryJson = MyMemory.FromJson(body);
                try
                {
                    translatedText = myMemoryJson.Matches.First(x => x.Reference?
                    .Equals("Machine Translation provided by Google, Microsoft, Worldlingo or MyMemory customized engine.")
                    ?? false).Translation;
                }
                catch (InvalidOperationException)
                {
                    translatedText = myMemoryJson.Matches.First().Translation;
                }
            }

            CacheTranslation(inputText, sourceLanguageCode, translatedText, destinationLanguageCode);
            return translatedText;
        }

        private static Cache CacheTranslation(string inputText, string sourceLanguageCode, string translatedText, string destinationLanguageCode)
        {
            var translationFilePath = GetTranslationsFilePath();
            var translationFileContents = File.ReadAllBytes(translationFilePath);
            var translation = Translation.FromJson(Encoding.UTF8.GetString(translationFileContents));
            var nextId = translation.Cache.Last().Id + 1;
            var newCachedText = new Cache()
            {
                Id = nextId,
                InputText = inputText,
                SourceLanguage = sourceLanguageCode,
                OutputText = translatedText,
                DestinationLanguage = destinationLanguageCode
            };

            translation.Cache = translation.Cache.Append(newCachedText).ToArray();

            var serializedTranslation = Translation.ToJson(translation);
            File.WriteAllBytes(translationFilePath, Encoding.UTF8.GetBytes(serializedTranslation));
            return newCachedText;
        }

        private static string GetCachedTranslation(string inputText, string sourceLanguageCode, string destinationLanguageCode)
        {
            var translationFilePath = GetTranslationsFilePath();
            var translationFileContents = File.ReadAllBytes(translationFilePath);
            var translation = Translation.FromJson(Encoding.UTF8.GetString(translationFileContents));
            var cachedTranslation = translation.Cache.SingleOrDefault(x => 
            x.InputText.Equals(inputText) && 
            x.SourceLanguage.Equals(sourceLanguageCode) && 
            x.DestinationLanguage.Equals(destinationLanguageCode));

            return cachedTranslation?.OutputText;
        }

        private static string GetTranslationsFilePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", nameof(Properties.Resources.Translations) + ".json");
        }

        private static Uri GetTranslationUri(string inputText, string sourceLanguageCode, string destinationLanguageCode, string translatorMail = "")
        {
            return new Uri($"https://api.mymemory.translated.net/get?q={inputText}&langpair={sourceLanguageCode}|{destinationLanguageCode}" +
                $"{(!translatorMail.Equals(string.Empty) ? $"&de={translatorMail}" : "")}");
        }
    }
}
