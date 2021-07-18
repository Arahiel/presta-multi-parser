namespace ECommerceParser.Model.Prestashop
{
    public class AttributeValue
    {
        public uint Position { get; set; }
        public string Value { get; set; }
        public AttributeValue(uint position, string value)
        {
            Position = position;
            Value = value;
        }
        public override string ToString()
        {
            return $"{Value}:{Position}";
        }
    }
}