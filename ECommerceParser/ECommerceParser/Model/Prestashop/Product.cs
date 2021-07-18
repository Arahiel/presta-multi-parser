using ECommerceParser.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Prestashop
{
    public class Product : GenericProduct
    {
        public int Id { get; }
        public string Reference { get; }
        public int PriceTaxIncluded { get; set; }
        public int TaxRule{ get; set; }
        public int CostPrice{ get; set; }


    }
}
