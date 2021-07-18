using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Prestashop
{
    public class ProductVariant
    {
        private readonly Product _product;

        public ProductVariant(Product product)
        {
            _product = product;
        }
    }
}
