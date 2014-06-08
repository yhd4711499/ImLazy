using System.Collections.Generic;
using System.Windows.Controls;
using ImLazy.SDK.Base.Contracts;

namespace ImLazy.Addins.ContentViews
{
    class DateTimeContent : TextBox, IEditView
    {
        public SerializableDictionary<string, object> Configuration { get; set; }
    }
}
