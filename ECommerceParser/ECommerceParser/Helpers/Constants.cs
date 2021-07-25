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
        public static readonly double Margin = 0.5;
    }
}
