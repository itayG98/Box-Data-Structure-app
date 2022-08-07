﻿using System;
using Windows.UI.Xaml.Data;

namespace Boxes_Store_app
{

    internal class StringFormatter : IValueConverter
    {
        //Converter={StaticResource StringFormatterValueConverter}, ConverterParameter=  Width - {0:0.00}}"

        // No need to implement converting back on a one-way binding
        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotImplementedException();
        // This converts the value object to the string to display. This will work with most simple types.
        public object Convert(object value, Type targetType,object parameter, string language)
        {
            // Retrieve the format string and use it to format the value.
            string formatString = parameter as string;
            if (!string.IsNullOrEmpty(formatString))
                return string.Format(formatString, value);

            // If the format string is null or empty, simply call ToString() on the value.
            return value.ToString();
        }
    }
}
