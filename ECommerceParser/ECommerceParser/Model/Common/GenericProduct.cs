using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Common
{
    public abstract class GenericProduct
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Categories Categories { get; set; }
        public ImageUrls ImageUrls { get; set; }
        public Tags Tags { get; set; }
    }
}
