using ECommerceParser.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Common
{
    public class Category : INotifyPropertyChanges
    {
        public readonly Category Parent;
        public static Category Root { get; } = new Category();
        public string Name
        {
            get => _name;
            set
            {
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Name)));
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        private static Lazy<CategoryBuilder> _builder = new Lazy<CategoryBuilder>(() => new CategoryBuilder());
        private string _name;

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        public static CategoryBuilder Builder => _builder.Value;

        private Category()
        {
        }

        public Category(Category parent, string name)
        {
            Parent = parent;
            Name = name;
        }

        public static Category FromString(string categoryString)
        {
            var cb = new CategoryBuilder();

            foreach (var categoryName in categoryString.Split('/'))
            {
                var input = categoryName.Trim();
                var firstUpperLetter = input[0].ToString().ToUpper();
                var rest = input.Substring(1);

                cb.AddCategory(firstUpperLetter + rest);
            }

            return cb.Build();
        }

        public override string ToString()
        {
            var parentName = Parent != null ? (Parent.Name != null ? Parent.ToString() + "/" : string.Empty) : string.Empty;
            return parentName + Name;
        }
    }
}
