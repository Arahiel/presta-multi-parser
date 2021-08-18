using ECommerceParser.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Common
{
    public class Categories : ObservableCollection<Category>
    {
        public event PropertyChangedEventHandler CategoryChanged;
        public event PropertyChangingEventHandler CategoryChanging;

        public Categories()
        {
            CollectionChanged += OnCategoriesUpdated;
        }

        public Categories(List<Category> categories) : base(categories)
        {
            CollectionChanged += OnCategoriesUpdated;
        }

        public Categories(string categoriesString) : base(categoriesString.Split(',').Select(x => Category.FromString(x)).ToList())
        {
            CollectionChanged += OnCategoriesUpdated;
        }

        public override string ToString()
        {
            return string.Join(",", this);
        }

        public static Categories FromListWithUpdatedHandler(List<Category> categories)
        {
            var newCategories = new Categories();

            foreach (var category in categories)
            {
                newCategories.Add(category);
            }

            return newCategories;
        }

        private void OnCategoriesUpdated(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanges item in e.OldItems)
                {
                    item.PropertyChanging -= item_PropertyChanging;
                    item.PropertyChanged -= item_PropertyChanged;
                }
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanges item in e.NewItems)
                {
                    item.PropertyChanging += item_PropertyChanging;
                    item.PropertyChanged += item_PropertyChanged;
                }
            }
        }

        private void item_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (!(sender is Category o)) return;
            CategoryChanging?.Invoke(sender, e);

        }

        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is Category o)) return;
            CategoryChanged?.Invoke(sender, e);
        }
    }
}
