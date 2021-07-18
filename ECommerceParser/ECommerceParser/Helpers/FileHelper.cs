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
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                return File.ReadAllText(openFileDialog.FileName);
            return null;
        }

        public static bool SaveFileDialog(string input)
        {
            var saveFileDialog = new SaveFileDialog();
            bool? ok = saveFileDialog.ShowDialog();
            if (!ok.HasValue) return false;
            if (ok.Value) File.WriteAllText(saveFileDialog.FileName, input);
            return ok.Value;
        }
    }
}
