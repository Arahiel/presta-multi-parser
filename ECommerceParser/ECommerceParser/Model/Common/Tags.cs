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
    public class Tags : ObservableCollection<Tag>
    {
        public event PropertyChangedEventHandler TagChanged;
        public event PropertyChangingEventHandler TagChanging;

        public Tags()
        {
            CollectionChanged += OnTagUpdated;
        }

        public Tags(List<Tag> tags) : base(tags)
        {
            CollectionChanged += OnTagUpdated;
        }

        public Tags(string tagsString) : this(tagsString.Split(',').Select(x => new Tag(x)).ToList())
        {
            CollectionChanged += OnTagUpdated;
        }

        public static Tags FromListWithUpdatedHandler(List<Tag> tags)
        {
            var newTags = new Tags();

            foreach (var tag in tags)
            {
                newTags.Add(tag);
            }

            return newTags;
        }

        public override string ToString()
        {
            return string.Join(",", this.Select(x => x.Name));
        }

        private void OnTagUpdated(object sender, NotifyCollectionChangedEventArgs e)
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
            if (!(sender is Tag o)) return;
            TagChanging?.Invoke(sender, e);

        }

        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is Tag o)) return;
            TagChanged?.Invoke(sender, e);
        }
    }
}
