using System;
using ImLazy.Addins.Utils;
using ImLazy.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using ImLazy.Util;
using ImLazy.RunTime;
using WpfLocalization;

namespace ImLazy.Addins.Conditions
{
    /// <summary>
    /// SimpleMetadataConditionAddinView.xaml 的交互逻辑
    /// </summary>
    public partial class FileTypeConditionAddinView : IEditView
    {
        SerializableDictionary<string, object> _configuration;

        static FileTypeConditionAddinView()
        {
        }

        public FileTypeConditionAddinView()
        {
            InitializeComponent();
            cmb_FileTypes.ItemsSource = SimpleMetadataConditionAddin.FileTypes.Select(_ => _.Key.LocalString());
        }

        public SerializableDictionary<string, object> Configuration
        {
            get
            {
                _configuration = new SerializableDictionary<string, object>
                {
                        {ConfigNames.FileType, ((LocalString) cmb_FileTypes.SelectedItem).Value}
                    };
                return _configuration;
            }
            set
            {
                if (_configuration == value)
                    return;
                _configuration = value;
                UpdateForm();
            }
        }

        void UpdateForm()
        {
            if (_configuration == null) return;
            cmb_FileTypes.SelectItem(_configuration.TryGetValue<string>(ConfigNames.FileType));
        }
    }
}
