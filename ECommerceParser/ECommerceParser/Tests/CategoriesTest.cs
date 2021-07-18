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
    public class CategoriesTest
    {
        [Test]
        public void CategoryToStringGivesCorrectCategoryTreeOutput()
        {
            var parentCategory = new Category(Category.Root, "Painted picture");
            var childCategory = new Category(parentCategory, "Flowers");

            var output = childCategory.ToString();

            Assert.That(output, Is.EqualTo($"{parentCategory.Name}/{childCategory.Name}"));
        }

        [Test]
        public void CategoryBuilderCreatesCategoryTreeProperly()
        {
            var cb = Category.Builder;

            const string parentCategoryName = "Painted picture";
            const string childCategoryName = "Flowers";

            var output = cb.AddCategory(parentCategoryName)
                .AddCategory(childCategoryName)
                .Build()
                .ToString();

            Assert.That(output, Is.EqualTo($"{parentCategoryName}/{childCategoryName}"));
        }
    }
}
