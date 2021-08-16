using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Helpers
{
    public static class FileHelper
    {
        public static string ReadFileDialog()
        {
            var openFileDialog = new OpenFileDialog()
            {
                Filter = "Comma-separated files (*.csv)|*.csv|All files (*.*)|*.*",
                Title = "Choose file to import"
            };
            if (openFileDialog.ShowDialog() == true)
                return File.ReadAllText(openFileDialog.FileName);
            return null;
        }

        public static bool SaveFileDialog(string productsInput, string productVariantsInput)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Filter = "Comma-separated files (*.csv)|*.csv|All files (*.*)|*.*",
                Title = "Choose file to export"
            };
            bool? ok = saveFileDialog.ShowDialog();
            if (!ok.HasValue) return false;
            if (ok.Value) File.WriteAllText(saveFileDialog.FileName, productsInput);

            var nameStrings = saveFileDialog.SafeFileName.Split('.');
            var fileName = nameStrings[0];
            var extension = nameStrings[1];
            var variantsFilePath = saveFileDialog.FileName.Replace(saveFileDialog.SafeFileName, $"{fileName}.combinations.{extension}");

            if (ok.Value) File.WriteAllText(variantsFilePath, productVariantsInput);
            return ok.Value;
        }
    }
}
