using ECommerceParser.Helpers;
using ECommerceParser.Model.Artb2b;
using ECommerceParser.Model.Common;
using ECommerceParser.Model.Prestashop;
using ECommerceParser.Parsers.Artb2b;
using EuropeanCentralBank.ExchangeRates;
using Google.Cloud.Translation.V2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        private ICommand __translateCommand;
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

        public ICommand TranslateCommand
        {
            get
            {
                if (__translateCommand == null)
                {
                    __translateCommand = new RelayCommand(
                        param => TranslateCurrentFile()
                    );
                }
                return __translateCommand;
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
        private string _currentSourceLanguageCode;
        private string _currentDestinationLanguageCode;
        private Task<ExportedProductsFile> _currentTranslationTask;
        private bool _areControlsEnabled;
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

        public ExportedProductsFile CurrentExportedProductsFile { get; set; }
        public ExportedProductVariantsFile CurrentExportedProductVariantsFile { get; set; }
        public ObservableCollection<ExportedProduct> CurrentExportedProducts { get; set; } = new ObservableCollection<ExportedProduct>();
        public ObservableCollection<ExportedProductVariant> CurrentExportedProductVariants { get; set; } = new ObservableCollection<ExportedProductVariant>();
        public List<string> LanguageCodeList { get; }
        public string CurrentSourceLanguageCode
        {
            get => _currentSourceLanguageCode;
            set
            {
                if (value == null) return;
                _currentSourceLanguageCode = typeof(LanguageCodes).GetField(value).GetValue(default(LanguageCodes)).ToString();
            }
        }
        public string CurrentDestinationLanguageCode
        {
            get => _currentDestinationLanguageCode;
            set
            {
                if (value == null) return;
                _currentDestinationLanguageCode = typeof(LanguageCodes).GetField(value).GetValue(default(LanguageCodes)).ToString();
            }
        }

        public Currencies CurrentSourceCurrency { get; set; }
        public Currencies CurrentDestinationCurrency { get; set; }
        public bool AreControlsEnabled { 
            get => _areControlsEnabled;
            set
            {
                _areControlsEnabled = value;
                OnPropertyChanged(MethodBase.GetCurrentMethod().Name.Substring(4));
            }
        }
        #endregion

        #region Constructors
        public ECommerceParserViewModel()
        {
            LanguageCodeList = typeof(LanguageCodes).GetFields().Select(x => x.Name).ToList();
            CurrentSourceCurrency = Currencies.PolishZloty;
            CurrentDestinationCurrency = Currencies.Euro;
            AreControlsEnabled = true;
        }
        #endregion

        #region AppMethods
        public void ImportFile()
        {
            InputFileTextBoxValue = FileHelper.ReadFileDialog();
        }

        public async Task ParseProducts()
        {
            if (InputFileTextBoxValue == null || InputFileTextBoxValue.Equals(string.Empty))
            {
                MessageBox.Show("Input text is empty!\nPlease import a file.", "Empty input text box", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CurrentSourceLanguageCode == null)
            {
                MessageBox.Show("Source language is empty!\n" +
                    $"Please choose source language.", "Empty source language", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var importedFile = ImportedFile.Load(InputFileTextBoxValue.Split('\n'), CurrentSourceCurrency);
            var parser = new ProductParser(CurrentDestinationCurrency);
            (CurrentExportedProductsFile, CurrentExportedProductVariantsFile) = await parser.ParseProducts(importedFile, CurrentSourceLanguageCode);
            CurrentExportedProducts.Clear();
            foreach (var product in CurrentExportedProductsFile.Products)
            {
                CurrentExportedProducts.Add(product);
            }

            foreach (var productVariant in CurrentExportedProductVariantsFile.ProductVariants)
            {
                CurrentExportedProductVariants.Add(productVariant);
            }
        }

        public async Task TranslateCurrentFile()
        {
            if (CurrentExportedProductsFile == null)
            {
                MessageBox.Show("Datagrid is empty!\n" +
                    $"Please parse imported file.", "Empty datagrid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CurrentDestinationLanguageCode == null)
            {
                MessageBox.Show("Destination language is empty!\n" +
                    $"Please choose destination language.", "Empty destination language", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            AreControlsEnabled = false;

            _currentTranslationTask = Translator.Translate(CurrentExportedProductsFile, LanguageCodes.English);
            CurrentExportedProductsFile = await _currentTranslationTask;
            CurrentExportedProducts.Clear();
            foreach (var product in CurrentExportedProductsFile.Products)
            {
                CurrentExportedProducts.Add(product);
            }
            AreControlsEnabled = true;
        }

        public void ExportFiles()
        {
            if (CurrentExportedProductsFile == null)
            {
                MessageBox.Show("Datagrid is empty!\n" +
                    $"Please parse imported file.", "Empty datagrid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string productFileCsvString = CurrentExportedProductsFile.ToCsv();
            string productVariantsFileCsvString = CurrentExportedProductVariantsFile.ToCsv();

            FileHelper.SaveFileDialog(productFileCsvString, productVariantsFileCsvString);
        }
        #endregion
    }
}
