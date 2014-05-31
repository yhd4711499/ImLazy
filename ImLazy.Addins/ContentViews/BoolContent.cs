using System.Collections.Generic;
using ImLazy.SDK.Base.Contracts;
using System.Windows.Controls;
using ImLazy.Runtime;
using ImLazy.Util;

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
