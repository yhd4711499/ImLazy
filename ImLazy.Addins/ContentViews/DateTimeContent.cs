using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ImLazy.SDK.Base.Contracts;

namespace ImLazy.Addins.ContentViews
{
    class DateTimeContent : TextBox, IEditView
    {
        public SerializableDictionary<string, object> Configuration { get; set; }
    }
}
