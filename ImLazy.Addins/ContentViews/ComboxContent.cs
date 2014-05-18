using System.Collections.Generic;
using System.Linq;
using ImLazy.Addins.Utils;
using ImLazy.SDK.Base.Contracts;
using System.Windows.Controls;
using ImLazy.RunTime;
using ImLazy.SDK.Lexer;
using ImLazy.Util;

namespace ImLazy.Addins.ContentViews
{
    class ComboxContent : ComboBox, IEditView
    {
        private IEnumerable<LexerType> types;
        public SerializableDictionary<string, object> Configuration
        {
            get
            {
                return new SerializableDictionary<string, object>
                {
                    { ConfigNames.ObjectValue, types.ElementAt(SelectedIndex).CanonicalName }
                };
            }
            set
            {
                types = value.TryGetValue<IEnumerable<LexerType>>(ConfigNames.AvailableItems);
                ItemsSource = types.Select(_ => _.Name.LocalString());
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
