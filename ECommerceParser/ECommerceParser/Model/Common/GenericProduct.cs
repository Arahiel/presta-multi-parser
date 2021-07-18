using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Common
{
    public class GenericProduct
    {
        string Name { get; set; }
        string Description { get; set; }
        List<Category> Categories { get; set; }
        List<string> ImageUrls { get; set; }
        List<Tag> Tags { get; set; }
    }
}
