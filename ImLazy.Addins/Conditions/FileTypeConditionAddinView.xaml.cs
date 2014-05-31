using System.Collections.Generic;
using ImLazy.Addins.Utils;
using ImLazy.SDK.Base.Contracts;
using System.Linq;
using ImLazy.Runtime;
using WpfLocalization;
using ImLazy.Util;

namespace ImLazy.Addins.Conditions
{
    /// <summary>
    /// SimpleMetadataConditionAddinView.xaml 的交互逻辑
    /// </summary>
    public partial class FileTypeConditionAddinView : IEditView
    {
        SerializableDictionary<string, object> _configuration;

        public FileTypeConditionAddinView()
        {
            InitializeComponent();
            CmbFileTypes.ItemsSource = SimpleMetadataConditionAddin.FileTypes.Select(_ => _.Key.LocalString());
        }

        public SerializableDictionary<string, object> Configuration
        {
            get
            {
                _configuration = new SerializableDictionary<string, object>
                {
                    {ConfigNames.FileType, ((LocalString) CmbFileTypes.SelectedItem).Value}
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
            CmbFileTypes.SelectItem(_configuration.TryGetValue<string>(ConfigNames.FileType));
        }
    }
}
