using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Common
{
    public class ImageUrls : List<string>
    {
        public ImageUrls(IEnumerable<string> urls) : base(urls)
        {
        }

        public override string ToString()
        {
            return string.Join(",", this);
        }
    }
}
