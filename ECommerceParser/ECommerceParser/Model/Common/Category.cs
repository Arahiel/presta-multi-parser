using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Common
{
    public class Category
    {
        public readonly Category Parent;
        public static Category Root { get; } = new Category();
        public string Name { get; set; }

        private Category()
        {
        }

        public Category(Category parent, string name)
        {
            Parent = parent;
            Name = name;
        }

        public override string ToString()
        {
            var parentName = Parent != null ? (Parent.Name != null ? Parent.ToString() + "/" : string.Empty) : string.Empty;
            return  parentName + Name; 
        }
    }
}
