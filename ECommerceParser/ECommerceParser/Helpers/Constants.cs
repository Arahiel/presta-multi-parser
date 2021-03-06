using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Helpers.Wallpics
{
    public static class Constants
    {
        /// <summary>
        /// Key is the Tax percentage presented as int value
        /// Value is the Tax rule
        /// </summary>
        public static readonly Dictionary<int, int> WallpicsTaxRules = new Dictionary<int, int>
        {
            {23, 1}
        };

        /// <summary>
        /// Margin per product
        /// </summary>
        public static readonly double Margin = 1;

        /// <summary>
        /// Mail used for a mymemory.translated.net translator. Used to extend translation limit from 1000 (anonymous) to 10000 words/day.
        /// </summary>
        public static string TranslatorMail = "admin@wallpics.store";
    }
}
