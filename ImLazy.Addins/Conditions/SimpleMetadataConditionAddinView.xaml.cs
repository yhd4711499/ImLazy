using System;
using ImLazy.Addins.Utils;
using ImLazy.SDK.Base.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using ImLazy.Util;
using ImLazy.Runtime;
using WpfLocalization;

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
        private static readonly Dictionary<LocalString, SimpleMetadataConditionAddin.PropertyOpeartion> PropertyItemsSource = new Dictionary<LocalString, SimpleMetadataConditionAddin.PropertyOpeartion>();
        static SimpleMetadataConditionAddinView()
        {
            SimpleMetadataConditionAddin.PropertyOpeartions.ForEach(_ =>
            {
                PropertyItemsSource[_.Key.LocalString()] = _.Value;
            });
        }

        public SimpleMetadataConditionAddinView()
        {
            InitializeComponent();
            CmbProperties.ItemsSource = PropertyItemsSource;
            CmbMode.ItemsSource = Enum.GetNames(typeof(SimpleMetadataConditionAddin.MatchMode)).Select(_ => _.LocalString());
            //cmb_Properties.DisplayMemberPath = "Key";
        }

        public SerializableDictionary<string, object> Configuration
        {
            get
            {
                _configuration = new SerializableDictionary<string, object>
                {
                        {ConfigNames.TargetObject,((IEditView)ContentParam.Content).Configuration.TryGetValue(ConfigNames.TargetObject)},
                        {ConfigNames.Symbol,((LocalString) CmbAvailSymbols.SelectedItem).Value},
                        {ConfigNames.TargetProperty, GetSelectedPropertyPair().Key.Value},
                        {ConfigNames.Mode, ((LocalString) CmbMode.SelectedItem).Value},
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

        private KeyValuePair<LocalString, SimpleMetadataConditionAddin.PropertyOpeartion> GetSelectedPropertyPair()
        {
            return (KeyValuePair<LocalString, SimpleMetadataConditionAddin.PropertyOpeartion>)CmbProperties.SelectedItem;
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
            ContentParam.Content = ContentAddin.CreateMainView(dic);
        }

        /// <summary>
        /// Update PropertyItemsSource of cmb_AvailSymbols if dirty.
        /// </summary>
        /// <param name="propertyOpeartion"><see cref="SimpleMetadataConditionAddin.PropertyOpeartion"/></param>
        void RefreshAvailSymbols(SimpleMetadataConditionAddin.PropertyOpeartion propertyOpeartion)
        {
            if (!_isDirty)
                return;
            _isDirty = false;
            var symbols = SimpleMetadataConditionAddin.GetAvailSymbols(propertyOpeartion).Select(_ => _.Key.LocalString()).ToList();
            CmbAvailSymbols.ItemsSource = symbols;
            CmbAvailSymbols.SelectedIndex = 0;
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

            CmbProperties.SelectedItem = PropertyItemsSource.FirstOrDefault(_ => _.Key.Value.Equals(name));

            RefreshAvailSymbols(propertyPair);
            CmbAvailSymbols.SelectItem(_configuration.TryGetValue<string>(ConfigNames.Symbol));

            CmbMode.SelectItem(_configuration.TryGetValue<string>(ConfigNames.Mode));

            ContentParam.Content = ContentAddin.CreateMainView(new SerializableDictionary<string, object>
            {
                {ConfigNames.TargetObject, _configuration.TryGetValue(ConfigNames.TargetObject)},
                {ContentProviderAddin.TargetType, propertyPair.TargetType.FullName}
            });
        }
    }
}
