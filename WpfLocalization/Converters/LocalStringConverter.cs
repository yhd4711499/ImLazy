using System;
using System.Globalization;
using System.Resources;
using System.Windows.Data;

namespace WpfLocalization.Converters
{
    public class LocalStringConverter : IValueConverter
    {
        public ResourceManager ResourceManager { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = ResourceManager.GetString((string)value);
            return s ?? String.Format("[{0}]", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
