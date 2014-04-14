//using ImLazy.Contracts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

//namespace ImLazy.Addins
//{
//    /// <summary>
//    /// ExtensionConditionView.xaml 的交互逻辑
//    /// </summary>
//    partial class FilePropertyConditionAddinView : UserControl, IEditView
//    {
//        FilePropertyConditionAddin _addin;
//        IEnumerable<IConditionAddin> _conditions;

//        internal FilePropertyConditionAddinView(FilePropertyConditionAddin addin)
//        {
//            InitializeComponent();
//            _addin = addin;
//            _conditions = _addin.AvailConditions.Select(_ => _.Value);
//            this.Loaded += ExtensionConditionView_Loaded;
//        }

//        void ExtensionConditionView_Loaded(object sender, RoutedEventArgs e)
//        {
//            UpdateListBox();
//            UpdateForm();
//            //this.DataContext.ToString();
//        }

//        private void UpdateForm()
//        {
//            if (_configuration != null)
//            {
//                var s = _conditions.Where(_ => _.GetType().FullName.Equals(_configuration["Selected"])).FirstOrDefault();
//                this.availCheckers.SelectedItem = s;
//                this.availSymbols.SelectedItem = _configuration["Symbol"];
//                this.param.Text = _configuration["Param"].ToString();
//            }
//        }

//        void UpdateListBox()
//        {
//            this.availCheckers.ItemsSource = _conditions;
//        }

//        private Dictionary<string, object> _configuration;
//        public Dictionary<string, object> Configuration
//        {
//            get
//            {
//                _configuration = new SerializableDictionary<string, object>(){
//                        {"Selected",this.availCheckers.SelectedItem.ToString()},
//                        {"Symbol",this.availSymbols.SelectedItem.ToString()},
//                        {"Param",this.param.Text}
//                    };
//                return _configuration;
//            }
//            set
//            {
//                if (_configuration == value)
//                    return;
//                _configuration = value;
//                UpdateForm();
//            }
//        }

//        public IAddin Addin
//        {
//            get { return _addin; }
//        }
//    }
//}
