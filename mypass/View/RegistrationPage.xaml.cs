using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace mypass.View
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : UserControl
    {

        private bool _isPasswordEmpty = true;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsPasswordEmpty
        {
            get => _isPasswordEmpty;
            set
            {
                if (_isPasswordEmpty != value)
                {
                    _isPasswordEmpty = value;
                    OnPropertyChanged();
                }
            }
        }
        public RegistrationPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            IsPasswordEmpty = string.IsNullOrEmpty(passwordBox.Password);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
