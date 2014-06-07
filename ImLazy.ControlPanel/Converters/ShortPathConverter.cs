using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ImLazy.ControlPanel.Converters
{
    public class ShortPathConverter : StringConverterBase
    {
        protected override object Convert(string value)
        {
            var splits = value.Split('\\');
            return splits.Length > 2 ? splits.Last() : value;
        }
    }
}
