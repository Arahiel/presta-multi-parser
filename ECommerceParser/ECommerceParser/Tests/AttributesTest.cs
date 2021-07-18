using NUnit.Framework;
using ECommerceParser.Model.Prestashop;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceParser.Helpers.Enums.Prestashop;

namespace ECommerceParser.Tests
{
    [TestFixture]
    public class AttributesTest
    {
        [Test]
        public void AttributeToStringGivesCorrectOutput()
        {
            var ab = new Attribute("Layout", AttributeType.Select, 0, new AttributeValue(0, "5H"));

            var output = ab.ToString();

            Assert.That(output, Is.EqualTo($"{ab.Name}:{ab.Type.ToString().ToLower()}:{ab.Position}"));
        }

        [Test]
        public void AttributeValueToStringGivesCorrectOutput()
        {
            var ab = new Attribute("Layout", AttributeType.Select, 0, new AttributeValue(0, "5H"));

            var output = ab.AttributeValue.ToString();

            Assert.That(output, Is.EqualTo($"{ab.AttributeValue.Value}:{ab.AttributeValue.Position}"));
        }
    }
}
