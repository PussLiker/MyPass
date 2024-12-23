using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace mypass.View
{
    public partial class PassCheck : UserControl
    {
        public PassCheck()
        {
            InitializeComponent();
        }

        private void OnCheckPasswordClick(object sender, RoutedEventArgs e)
        {
            string password = PasswordTextBox.Text;

        }
    }
}
