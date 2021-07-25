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
        public List<Category> Categories { get; set; }
        public List<string> ImageUrls { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
