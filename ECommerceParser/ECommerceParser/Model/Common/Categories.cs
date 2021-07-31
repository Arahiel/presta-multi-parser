using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Common
{
    public class Categories : List<Category>
    {
        public Categories()
        {

        }

        public Categories(List<Category> categories) : base(categories)
        {

        }

        public Categories(string categoriesString) : base(categoriesString.Split(',').Select(x => Category.FromString(x)).ToList())
        {
        }

        public override string ToString()
        {
            return string.Join(",", this);
        }
    }
}
