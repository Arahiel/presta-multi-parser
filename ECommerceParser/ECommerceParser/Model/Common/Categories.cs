using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Common
{
    public class Categories : List<Category>
    {
        public override string ToString()
        {
            return string.Join(",", this);
        }
    }
}
