using mypass.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace mypass.View
{
    public partial class ErrorWindow : Window
    {
        public ErrorWindow():this("Произошла ошибка")
        {
            
        }
        public ErrorWindow(string errorMessage)
        {
            InitializeComponent();
            var viewModel = new ErrorWinVM(errorMessage)
            {
                CloseAction = Close
            };

            DataContext = viewModel;
        }

        public static void ShowError(string errorMessage)
        {
            var window = new ErrorWindow(errorMessage);
            window.ShowDialog();
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
    