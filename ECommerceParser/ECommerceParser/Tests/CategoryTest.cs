using ECommerceParser.Model.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Tests
{
    [TestFixture]
    public class CategoryTest
    {
        [Test]
        public void CategoryToStringGivesCorrectCategoryTreeOutput()
        {
            var parentCategory = new Category(Category.Root, "Painted picture");
            var childCategory = new Category(parentCategory, "Flowers");

            var output = childCategory.ToString();

            Assert.That(output, Is.EqualTo("Painted picture/Flowers"));
        }
    }
}
