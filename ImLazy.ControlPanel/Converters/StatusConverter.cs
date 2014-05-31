using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ImLazy.ControlPanel.Converters
{
    class StatusConverter: IValueConverter
    {
        public SolidColorBrush ErrorColor { get; set; }
        public SolidColorBrush NormalColor { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = value as Status;
            if(status == null)
                throw new ArgumentException("value");
            if (targetType == typeof(string))
            {
                return status.Message;
            }
            if (targetType == typeof(Brush))
            {
                return status.IsError
                    ? ErrorColor
                    : NormalColor;
            }
            throw new ArgumentException("targetType is not supported.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
