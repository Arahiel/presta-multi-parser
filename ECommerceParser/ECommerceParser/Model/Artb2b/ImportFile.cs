using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Artb2b
{
    public class ImportFile
    {
        public enum Headers
        {
            Id,
            Code,
            Name,
            Description,
            Price_with_tax,
            Tax,
            Purchase_price,
            Categories,
            Variants,
            Features,
            Images,
            Tags
        }
    }
}
