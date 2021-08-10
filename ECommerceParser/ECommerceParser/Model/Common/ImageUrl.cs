using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Common
{
    public class ImageUrl
    {
        public string Url { get; set; }

        public ImageUrl(string url)
        {
            Url = url;
        }

        public override string ToString()
        {
            return Url;
        }
    }
}
