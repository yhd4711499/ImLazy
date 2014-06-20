using System;
using System.Linq;

namespace ImLazy.ControlPanel.Converters
{
    public class ShortPathConverter : StringConverterBase
    {
        protected override object Convert(string value)
        {
            var splits = value.Split('\\');
            if (splits.Length < 2) return value;
            var last = splits.Last();
            return String.IsNullOrEmpty(last) ? value : last;
        }
    }
}
