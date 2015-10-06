using System.Collections.Generic;
using ImLazy.Runtime;
using ImLazy.SDK.Base.Contracts;
using ImLazy.Util;
using Technewlogic.WpfDialogManagement;
using ICSharpCode.AvalonEdit;
using System.Windows;
using System.Linq;
using Technewlogic.WpfDialogManagement.Contracts;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System.Xml;
using System.Diagnostics;

namespace ImLazy.Addins.ContentViews
{
    /// <summary>
    /// ScriptContent.xaml 的交互逻辑
    /// </summary>
    public partial class ScriptContent : IEditView
    {
        private TextEditor scriptEditor;
        private ICustomContentDialog dialog;
        private string script;
        public ScriptContent()
        {
            InitializeComponent();
        }

        private string scriptExt;
        public string ScriptExt
        {
            get{ return scriptExt; }
            set
            {
                if (scriptExt == value) return;
                scriptExt = value;
            }
        }

        public SerializableDictionary<string, object> Configuration
        {
            get
            {
                return new SerializableDictionary<string, object>
                {
                    {ConfigNames.ObjectValue, script}
                };
            }
            set { script = value.TryGetValue<string>(ConfigNames.ObjectValue); }
        }

        private void EditScript(object sender, System.Windows.RoutedEventArgs e)
        {
            scriptEditor = new TextEditor();
            scriptEditor.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Disabled;
            scriptEditor.TextChanged += ScriptEditor_TextChanged;
            scriptEditor.WordWrap = true;
            scriptEditor.Text = script;
            scriptEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(scriptExt);

            var dialogManager = new DialogManager(Application.Current.Windows[Application.Current.Windows.Count - 1], Dispatcher);
            dialog = dialogManager
                .CreateCustomContentDialog(scriptEditor, "Edit script", DialogMode.OkCancel);
            dialog.Ok = () => script = scriptEditor.Text;
            dialog.OkText = "保存";
            dialog.CancelText = "取消";
            dialog.CanOk = ValidateScript(script);
            dialog.Show();
        }

        private void ScriptEditor_TextChanged(object sender, System.EventArgs e)
        {
            if (dialog != null)
            {
                dialog.CanOk = ValidateScript(scriptEditor.Text);
            }
        }

        private bool ValidateScript(string script)
        {
            // TODO: validation
            return true;
        }
    }
}
