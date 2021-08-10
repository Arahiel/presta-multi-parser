using ECommerceParser.Helpers;
using ECommerceParser.Model.Common;
using ECommerceParser.Model.Prestashop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ECommerceParser.Controllers
{
    public class ECommerceParserViewModel : INotifyPropertyChanged
    {
        #region Commands
        #region #region PrivateCommandFields
        private ICommand __importFileCommand;
        private ICommand __parseProductsCommand;
        private ICommand __exportFileCommand;
        #endregion

        #region #region CommandProperties
        public ICommand ImportFileCommand
        {
            get
            {
                if (__importFileCommand == null)
                {
                    __importFileCommand = new RelayCommand(
                        param => ImportFile()
                    );
                }
                return __importFileCommand;
            }
        }

        public ICommand ParseProductsCommand
        {
            get
            {
                if (__parseProductsCommand == null)
                {
                    __parseProductsCommand = new RelayCommand(
                        param => ParseProducts()
                    );
                }
                return __parseProductsCommand;
            }
        }

        public ICommand ExportFileCommand
        {
            get
            {
                if (__exportFileCommand == null)
                {
                    __exportFileCommand = new RelayCommand(
                        param => ExportFiles()
                    );
                }
                return __exportFileCommand;
            }
        }
        #endregion
        #endregion

        #region INotifyPropertyMembers
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion

        #region AppPrivateFields
        private string _inputFileTextBoxValue;
        #endregion

        #region AppProperties
        public string InputFileTextBoxValue
        {
            get => _inputFileTextBoxValue;
            set
            {
                _inputFileTextBoxValue = value;
                OnPropertyChanged(MethodBase.GetCurrentMethod().Name.Substring(4));
            }
        }

        public ObservableCollection<ExportedProduct> CurrentExportedProducts { get; set; } = new ObservableCollection<ExportedProduct>();

        #endregion

        #region Constructors
        public ECommerceParserViewModel()
        {
        }
        #endregion

        #region AppMethods
        public void ImportFile()
        {
            //CurrentExportedProducts.Add(new ExportedProduct(1, "Ref-1,", 11, 1, 5, new Dictionary<string, string> { { "Layout", "5H" }, { "Width", "25" } })
            //{
            //    Name = "Name1",
            //    Categories = new Categories(new List<Category> { 
            //        new Category(Category.Root, "Parent1"), 
            //        new Category(Category.Root, "Parent2")}),
            //    Description = "Description",
            //    ImageUrls = new ImageUrls(new List<string> { "Url1", "Url2"}),
            //    Tags = new Tags("Tag1,Tag2,Tag3")
            //});

        }

        public void ParseProducts()
        {

        }

        public void ExportFiles()
        {

        }
        #endregion
    }
}
