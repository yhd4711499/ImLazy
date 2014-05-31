using System;
using ImLazy.Addins.Annotations;
using ImLazy.SDK.Base.Contracts;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using ImLazy.Util;
using ImLazy.Runtime;

namespace ImLazy.Addins
{
    /// <summary>
    /// MetadataConditionAddinView.xaml 的交互逻辑
    /// </summary>
    public partial class MetadataConditionAddinView : IEditView
    {
        static readonly ContentProviderAddin ContentAddin = new ContentProviderAddin();
        SerializableDictionary<string, object> _configuration;
        private bool _isDirty = true;
        /// <summary>
        /// All <see cref="MetadataConditionAddinView"/> should have the same Properties as items source.
        /// </summary>
        static readonly IEnumerable<string> ItemsSource = MetadataConditionAddin.Properties.Where(_ => _.CanonicalName != null).Select(_ => _.CanonicalName);
        public MetadataConditionAddinView()
        {
            InitializeComponent();
            CmbProperties.ItemsSource = ItemsSource;
        }

        public SerializableDictionary<string, object> Configuration
        {
            get
            {
                _configuration = new SerializableDictionary<string, object>
                {
                        {ConfigNames.TargetObject,((IEditView)ContentParam.Content).Configuration.TryGetValue(ConfigNames.TargetObject)},
                        {ConfigNames.Symbol,CmbAvailSymbols.SelectedItem.ToString()},
// ReSharper disable once RedundantCast
                        {ConfigNames.TargetProperty,(string)CmbProperties.SelectedItem}
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

        void cmb_Properties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _isDirty = true;
            var propertyName = (string)CmbProperties.SelectedItem;
            RefreshAvailSymbols(propertyName);
            var dic = new SerializableDictionary<string, object>
            {
                    {ContentProviderAddin.TargetType, SystemProperties.GetPropertyDescription(propertyName).ValueType.FullName}
                };
            if (_configuration != null)
            {
                dic.Add(ConfigNames.TargetObject, _configuration.TryGetValue(ConfigNames.TargetObject));
            }
            ContentParam.Content = ContentAddin.CreateMainView(dic);
        }

        /// <summary>
        /// Update ItemsSource of cmb_AvailSymbols if dirty.
        /// </summary>
        /// <param name="canonicalName">Full name of SystemProperty</param>
        void RefreshAvailSymbols([NotNull] string canonicalName)
        {
            if (canonicalName == null) throw new ArgumentNullException("canonicalName");
            if (!_isDirty)
                return;
            _isDirty = false;
            var symbols = MetadataConditionAddin.GetAvailSymbols(
                SystemProperties.GetPropertyDescription(canonicalName));
            CmbAvailSymbols.ItemsSource = symbols.Select(_ => _.Name);
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
            RefreshAvailSymbols(name);
            CmbAvailSymbols.SelectedItem = _configuration.TryGetValue<string>(ConfigNames.Symbol);
            var propertyName = _configuration.TryGetValue<string>(ConfigNames.TargetProperty);
            CmbProperties.SelectedItem = propertyName;
            ContentParam.Content = ContentAddin.CreateMainView(new SerializableDictionary<string, object>
            {
                {ConfigNames.TargetObject, _configuration.TryGetValue(ConfigNames.TargetObject)},
                {ContentProviderAddin.TargetType, SystemProperties.GetPropertyDescription(propertyName).ValueType.FullName}
            });
        }
    }
}
