using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Common
{
    public class Tag
    {
        public string Name { get; set; }

        public Tag(string name)
        {
            Name = name;
        }
    }
}
