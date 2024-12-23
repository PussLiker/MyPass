using System;
using System.Globalization;
using System.Windows.Data;

namespace mypass.Model
{
    public class PasswordToMaskedMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2
                && values[0] is string password
                && values[1] is bool isPasswordVisible)
            {
                // Возвращаем пароль, если он видим, иначе звёздочки
                return isPasswordVisible ? password : new string('•', password.Length);
            }
            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // Обратное преобразование не требуется
            return null;
        }
    }
}
