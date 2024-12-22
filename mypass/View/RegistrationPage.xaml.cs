using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using mypass.ViewModel;
using mypass.View;
using mypass.Model;
namespace mypass.View
{
    /// Логика взаимодействия для RegistrationPage.xaml
    public partial class RegistrationPage : UserControl
    {

        MainAuthWindowVM authWindow = new MainAuthWindowVM();

        public RegistrationPage()

        {
            InitializeComponent();
            DataContext = this;
        }



        public event PropertyChangedEventHandler PropertyChanged;


        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            authWindow.IsPasswordEmpty = string.IsNullOrEmpty(passwordBox.Password);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
