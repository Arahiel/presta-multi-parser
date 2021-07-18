namespace ECommerceParser.Model.Prestashop
{
    public class AttributeValue
    {
        public string Position { get; set; }
        public string Value { get; set; }
        public AttributeValue(string position, string value)
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