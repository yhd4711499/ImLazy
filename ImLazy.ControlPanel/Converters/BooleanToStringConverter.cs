using System;
using System.Globalization;
using ImLazy.ControlPanel.Util;

namespace ImLazy.ControlPanel.Converters
{
    public sealed class BooleanToStringConverter : BooleanConverter<string>
    {
        public BooleanToStringConverter() :
            base("True", "False") { }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = base.Convert(value, targetType, parameter, culture) as string;
            return str.Local();
        }
    }
}
