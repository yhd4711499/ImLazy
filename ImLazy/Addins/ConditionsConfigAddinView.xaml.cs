using ImLazy.Contracts;
using System;
using System.Collections.Generic;
using System.Windows;
using ImLazy.Util;
using ImLazy.RunTime;
using ImLazy.Data;

namespace ImLazy.Addins
{
    /// <summary>
    /// ConditionsConfigAddinView.xaml 的交互逻辑
    /// </summary>
    public partial class ConditionsConfigAddinView : IEditView
    {
        public ConditionsConfigAddinView()
        {
            InitializeComponent();
            Loaded += ConditionsConfigAddinView_Loaded;
        }

        void ConditionsConfigAddinView_Loaded(object sender, RoutedEventArgs e)
        {
            CmbMode.ItemsSource = Enum.GetNames(typeof(ConditionMode));
            if (CmbMode.SelectedItem == null)
            {
                CmbMode.SelectedIndex = 0;
            }
            
        }

        public SerializableDictionary<string, object> Configuration
        {
            get
            {
                var dic = new SerializableDictionary<string, object>
                { 
                    {ConfigNames.Symbol, CmbMode.SelectedItem}
                };
                return dic;
            }
            set
            {
                var modeStr = value.TryGetValue<string>(ConfigNames.Symbol);
                CmbMode.SelectedItem = modeStr;
            }
        }
    }
}
