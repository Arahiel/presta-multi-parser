using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Common
{
    public class ImageUrls : List<ImageUrl>
    {
        public ImageUrls(IEnumerable<ImageUrl> urls) : base(urls)
        {
        }

        public ImageUrls(IEnumerable<string> urls) : base(urls.Select(x => new ImageUrl(x)))
        {
        }

        public override string ToString()
        {
            return string.Join(",", this);
        }
    }
}
