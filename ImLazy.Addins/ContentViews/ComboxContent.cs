using System.Collections.Generic;
using System.Linq;
using ImLazy.Addins.Utils;
using ImLazy.SDK.Base.Contracts;
using System.Windows.Controls;
using ImLazy.Runtime;
using ImLazy.SDK.Lexer;
using ImLazy.Util;

namespace ImLazy.Addins.ContentViews
{
    class ComboxContent : ComboBox, IEditView
    {
        private IEnumerable<LexerType> _types;
        public SerializableDictionary<string, object> Configuration
        {
            get
            {
                return new SerializableDictionary<string, object>
                {
                    { ConfigNames.ObjectValue, _types.ElementAt(SelectedIndex).CanonicalName }
                };
            }
            set
            {
                _types = value.TryGetValue<IEnumerable<LexerType>>(ConfigNames.AvailableItems);
                ItemsSource = _types.Select(_ => _.Name.LocalString());
                var obj = value.TryGetValue<string>(ConfigNames.ObjectValue);
                if (obj != null)
                {
                    this.SelectItem(obj.Split('.').Last());
                }
                else
                {
                    SelectedIndex = 0;
                }
            }
        }
    }
}
