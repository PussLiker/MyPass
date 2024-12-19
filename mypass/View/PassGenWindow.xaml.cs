using System.Windows;
using System.Windows.Input;

namespace mypass.View
{
    /// <summary>
    /// Логика взаимодействия для PassGenWindow.xaml
    /// </summary>
    public partial class PassGenWindow : Window
    {
        public PassGenWindow()
        {
            InitializeComponent();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }


    }
}
