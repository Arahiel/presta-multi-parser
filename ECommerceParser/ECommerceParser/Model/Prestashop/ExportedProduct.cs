using ECommerceParser.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Prestashop
{
    public class ExportedProduct : GenericProduct
    {
        public int Id { get; }
        public string Reference { get; }
        public double PriceTaxIncluded { get; set; }
        public int TaxRule{ get; set; }
        public double CostPrice { get; set; }
        public List<ExportedProductVariant> Variants { get; set; }
        public Dictionary<string, string> Features { get; set; }

        public ExportedProduct(int id, string reference, double priceTaxIncluded, int taxRule, double costPrice, Dictionary<string, string> features)
        {
            Id = id;
            Reference = reference;
            PriceTaxIncluded = priceTaxIncluded;
            TaxRule = taxRule;
            CostPrice = costPrice;
            Features = features;
        }
    }
}
