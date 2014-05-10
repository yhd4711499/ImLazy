using ImLazy.Contracts;
using System.Collections.Generic;
using System.Windows.Controls;
using ImLazy.Util;
using ImLazy.RunTime;

namespace ImLazy.Addins.ContentViews
{
    class TextContent:TextBox,IEditView
    {
        public SerializableDictionary<string, object> Configuration
        {
            get
            {
                return new SerializableDictionary<string, object>
                {
                    {ConfigNames.ObjectValue,Text}
                };
            }
            set
            {
                Text = value.TryGetValue(ConfigNames.ObjectValue) as string;
            }
        }
    }
}
