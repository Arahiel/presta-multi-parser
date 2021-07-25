using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Artb2b
{
    public class ImportedProduct
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price_with_tax { get; set; }
        public int Tax { get; set; }
        public double Purchase_price { get; set; }
        public string Categories { get; set; }
        public string Variants { get; set; }
        public string Features { get; set; }
        public string Images { get; set; }
        public string Tags { get; set; }

        public ImportedProduct(int id, string code, string name, string description, double price_with_tax, int tax, double purchase_price, string categories, string variants, string features, string images, string tags)
        {
            Id = id;
            Code = code;
            Name = name;
            Description = description;
            Price_with_tax = price_with_tax;
            Tax = tax;
            Purchase_price = purchase_price;
            Categories = categories;
            Variants = variants;
            Features = features;
            Images = images;
            Tags = tags;
        }

    }
}
