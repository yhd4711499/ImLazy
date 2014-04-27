using ImLazy.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using ImLazy.Util;
using ImLazy.RunTime;

namespace ImLazy.Addins.Conditions
{
    /// <summary>
    /// SimpleMetadataConditionAddinView.xaml 的交互逻辑
    /// </summary>
    public partial class SimpleMetadataConditionAddinView : IEditView
    {
        static readonly ContentProviderAddin ContentAddin = new ContentProviderAddin();
        SerializableDictionary<string, object> _configuration;
        private bool _isDirty = true;

        /// <summary>
        /// All <see cref="SimpleMetadataConditionAddinView"/> should have the same Properties as items source.
        /// </summary>
        private static readonly Dictionary<string, SimpleMetadataConditionAddin.PropertyOpeartion> ItemsSource = SimpleMetadataConditionAddin.PropertyOpeartions;
        public SimpleMetadataConditionAddinView()
        {
            InitializeComponent();
            cmb_Properties.ItemsSource = ItemsSource;
            cmb_Properties.DisplayMemberPath = "Key";
        }

        public SerializableDictionary<string, object> Configuration
        {
            get
            {
                _configuration = new SerializableDictionary<string, object>
                {
                        {ConfigNames.TargetObject,((IEditView)content_param.Content).Configuration.TryGetValue(ConfigNames.TargetObject)},
                        {ConfigNames.Symbol,cmb_AvailSymbols.SelectedItem.ToString()},
// ReSharper disable once RedundantCast
                        {ConfigNames.TargetProperty, GetSelectedPropertyFullName()}
                    };
                return _configuration;
            }
            set
            {
                if (_configuration == value)
                    return;
                _configuration = value;
                _isDirty = true;
                UpdateForm();
            }
        }

        private string GetSelectedPropertyFullName()
        {
            return SimpleMetadataConditionAddin.GetPropertyOperationName(cmb_Properties.SelectedItem);
        }

        private KeyValuePair<string, SimpleMetadataConditionAddin.PropertyOpeartion> GetSelectedPropertyPair()
        {
            return (KeyValuePair<string, SimpleMetadataConditionAddin.PropertyOpeartion>)cmb_Properties.SelectedItem;
        }

        void cmb_Properties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _isDirty = true;
            var propertyPair = GetSelectedPropertyPair();
            RefreshAvailSymbols(propertyPair.Value);
            var dic = new SerializableDictionary<string, object>
            {
                    {ContentProviderAddin.TargetType, propertyPair.Value.TargetType.FullName}
                };
            if (_configuration != null)
            {
                dic.Add(ConfigNames.TargetObject, _configuration.TryGetValue(ConfigNames.TargetObject));
            }
            content_param.Content = ContentAddin.CreateMainView(dic);
        }

        /// <summary>
        /// Update ItemsSource of cmb_AvailSymbols if dirty.
        /// </summary>
        /// <param name="propertyOpeartion"><see cref="SimpleMetadataConditionAddin.PropertyOpeartion"/></param>
        void RefreshAvailSymbols(SimpleMetadataConditionAddin.PropertyOpeartion propertyOpeartion)
        {
            if (!_isDirty)
                return;
            _isDirty = false;
            var symbols = SimpleMetadataConditionAddin.GetAvailSymbols(propertyOpeartion);
            cmb_AvailSymbols.ItemsSource = symbols.Select(_ => _.Key);
            cmb_AvailSymbols.SelectedIndex = 0;
        }

        void UpdateForm()
        {
            if (_configuration == null) return;
            var name = _configuration.TryGetValue<string>(ConfigNames.TargetProperty);
            if (name == null)
            {
                return;
            }
            SimpleMetadataConditionAddin.PropertyOpeartion propertyPair;
            if (!SimpleMetadataConditionAddin.PropertyOpeartions.TryGetValue(name, out propertyPair))
            {
                return;
            }

            RefreshAvailSymbols(propertyPair);
            cmb_AvailSymbols.SelectedItem = _configuration.TryGetValue<string>(ConfigNames.Symbol);
            var selectedPo = ItemsSource.FirstOrDefault(_ => _.Key.Equals(name));
            cmb_Properties.SelectedItem = selectedPo;
            content_param.Content = ContentAddin.CreateMainView(new SerializableDictionary<string, object>
            {
                {ConfigNames.TargetObject, _configuration.TryGetValue(ConfigNames.TargetObject)},
                {ContentProviderAddin.TargetType, propertyPair.TargetType.FullName}
            });
        }
    }
}
