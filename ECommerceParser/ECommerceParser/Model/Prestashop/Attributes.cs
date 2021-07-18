using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Prestashop
{
    public class Attributes : List<Attribute>
    {
        public override string ToString()
        {
            return string.Join(",", this);
        }

        public string Values()
        {
            return string.Join(",", this.Select(x => x.AttributeValue));
        }
    }
}
