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

        }
    }
}
