using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace mypass.Model
{
    public class PasswordToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string password = value as string;
            return string.IsNullOrWhiteSpace(password) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}