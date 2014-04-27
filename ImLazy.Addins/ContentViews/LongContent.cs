using ImLazy.Contracts;
using System.Collections.Generic;
using System.Windows.Controls;
using ImLazy.Util;
using ImLazy.RunTime;

namespace ImLazy.Addins.ContentViews
{
    class LongContent : TextBox , IEditView
    {
        public SerializableDictionary<string, object> Configuration
        {
            get
            {
                return new SerializableDictionary<string, object>
                {
                    {ConfigNames.TargetObject, Text}
                };
            }
            set
            {
                Text = value.TryGetValue(ConfigNames.TargetObject) as string;
            }
        }
    }
}
