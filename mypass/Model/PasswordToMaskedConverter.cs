using System;
using System.Globalization;
using System.Windows.Data;

namespace mypass.Utilities
{
    internal class PasswordToMaskedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string password && parameter is bool isPasswordVisible)
            {
                return isPasswordVisible ? password : new string('•', password.Length);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ConvertBack не используется, можно оставить как есть
            return value;
        }
    }
}
