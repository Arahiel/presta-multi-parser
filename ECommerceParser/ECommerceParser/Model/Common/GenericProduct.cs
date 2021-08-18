using ECommerceParser.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Common
{
    public abstract class GenericProduct : INotifyPropertyChanges
    {
        private string name;
        private string description;
        private Categories categories;
        private ImageUrls imageUrls;
        private Tags tags;

        public string Name
        {
            get => name;
            set
            {
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Name)));
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }
        public string Description
        {
            get => description;
            set
            {
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Description)));
                description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            }
        }
        public Categories Categories
        {
            get => categories;
            set
            {
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Categories)));
                categories = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Categories)));
            }
        }
        public ImageUrls ImageUrls
        {
            get => imageUrls;
            set
            {
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(ImageUrls)));
                imageUrls = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageUrls)));
            }
        }
        public Tags Tags
        {
            get => tags;
            set
            {
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Tags)));
                tags = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tags)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
    }
}
