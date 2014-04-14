using ImLazy.Contracts;
using System.Collections.Generic;
using System.Windows.Controls;
using ImLazy.Util;
using ImLazy.RunTime;

namespace ImLazy.Addins.ContentViews
{
    class BoolContent : CheckBox,IEditView
    {
        public SerializableDictionary<string, object> Configuration
        {
            get
            {
                return new SerializableDictionary<string, object>
                {
                    {ConfigNames.TargetObject,IsChecked}
                };
            }
            set
            {
                IsChecked = value.TryGetValue<bool>(ConfigNames.TargetObject);
            }
        }
    }
}
