using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using mypass.ViewModel;
using mypass.View;
using mypass.Model;
using System;
using System.Text.RegularExpressions;
namespace mypass.View
{
    /// Логика взаимодействия для RegistrationPage.xaml
    public partial class RegistrationPage : UserControl
    {


        public RegistrationPage()

        {
            InitializeComponent();

        }

        private void UsernameTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var allowedChars = @"^[a-zA-Z0-9]*$";  // Разрешены только латинские буквы и цифры

            // Проверка, соответствует ли введенный символ разрешенным
            if (!Regex.IsMatch(e.Text, allowedChars))
            {
                e.Handled = true; // Если символ не разрешен, блокируем ввод
            }
        }
    }
}
