using ECommerceParser.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceParser.Model.Common
{
    public class Tag : INotifyPropertyChanges
    {
        private string _name;

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

        public Tag(string name)
        {
            var input = name.Trim();
            var firstUpperLetter = input[0].ToString().ToUpper();
            var rest = input.Substring(1);
            Name = firstUpperLetter + rest;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
    }
}
