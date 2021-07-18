using ECommerceParser.Helpers.Enums.Prestashop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Prestashop
{
    public class Attribute
    {
        public string Name { get; set; }
        public AttributeType Type { get; set; }
        public uint Position { get; set; }
        public AttributeValue AttributeValue { get; set; }

        public Attribute(string name, AttributeType type, uint position, AttributeValue attributeValue)
        {
            Name = name;
            Type = type;
            Position = position;
            AttributeValue = attributeValue;
        }

        public override string ToString()
        {
            return $"{Name}:{Type.ToString().ToLower()}:{Position}";
        }
    }
}
