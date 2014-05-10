using System.Collections.Generic;
using ImLazy.SDK.Base.Contracts;
using System.Windows.Controls;
using ImLazy.RunTime;
using ImLazy.Util;

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
