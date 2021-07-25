using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Prestashop
{
    public class ExportedProductVariant
    {
        public int Id { get; set; }
        public Attributes Attributes { get; set; }
        public string Reference { get; set; }
        public double ImpactOnPrice { get; set; }
        public int Quantity { get; set; }
        public bool Default { get; set; }

        public ExportedProductVariant(int id, Attributes attributes, string reference, double impactOnPrice, int quantity, bool _default)
        {
            Id = id;
            Attributes = attributes;
            Reference = reference;
            ImpactOnPrice = impactOnPrice;
            Quantity = quantity;
            Default = _default;
        }
    }
}
