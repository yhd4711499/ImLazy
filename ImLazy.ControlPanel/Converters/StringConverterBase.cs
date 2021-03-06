﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace ImLazy.ControlPanel.Converters
{
    public abstract class StringConverterBase : IValueConverter
    {
        protected abstract object Convert(string value);
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            return str == null ? null : Convert(str);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
