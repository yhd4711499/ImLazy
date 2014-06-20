using System.Collections.Generic;
using ImLazy.Runtime;
using ImLazy.SDK.Base.Contracts;
using ImLazy.Util;

namespace ImLazy.Addins.ContentViews
{
    /// <summary>
    /// ScriptContent.xaml 的交互逻辑
    /// </summary>
    public partial class ScriptContent : IEditView
    {
        public ScriptContent()
        {
            InitializeComponent();
        }

        public SerializableDictionary<string, object> Configuration
        {
            get
            {
                return new SerializableDictionary<string, object>
                {
                    {ConfigNames.ObjectValue, ScriptTextBox.Text}
                };
            }
            set { ScriptTextBox.Text = value.TryGetValue<string>(ConfigNames.ObjectValue); }
        }
    }
}
