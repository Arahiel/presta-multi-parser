namespace ECommerceParser.Model.Common
{
    public class CategoryBuilder
    {
        private readonly Category root;
        private Category actual;

        public CategoryBuilder()
        {
            root = Category.Root;
        }

        public CategoryBuilder(Category root)
        {
            this.root = root;
        }

        public CategoryBuilder AddCategory(string categoryName)
        {
            if (actual == null)
            {
                actual = new Category(root, categoryName);
            }
            else
            {
                actual = new Category(actual, categoryName);
            }
            return this;
        }

        public Category Build() => actual;

        public CategoryBuilder Clear()
        {
            actual = null;
            return this;
        }
    }
}