﻿using ImLazy.Contracts;
using System;
using System.Collections.Generic;
using System.Windows;
using ImLazy.Util;
using ImLazy.RunTime;
using ImLazy.Data;
using ImLazy.Util;

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
            // For localization
            var dic = new Dictionary<string, string>();
            Enum.GetNames(typeof(ConditionMode)).ForEach(_ => dic.Add(_.Local(), _));

            CmbMode.ItemsSource = dic;
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
                    {ConfigNames.Symbol, CmbMode.SelectedValue}
                };
                return dic;
            }
            set
            {
                var modeStr = value.TryGetValue<string>(ConfigNames.Symbol);
                CmbMode.SelectedValue = modeStr;
            }
        }
    }
}
