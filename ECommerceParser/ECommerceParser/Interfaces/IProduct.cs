using ECommerceParser.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Interfaces
{
    public class IProduct
    {
        string Name { get; set; }
        string Description { get; set; }
        List<Category> Categories { get; set; }
    }
}
