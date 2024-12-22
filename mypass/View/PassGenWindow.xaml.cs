using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace mypass.View
<<<<<<<< HEAD:mypass/View/PassGenWindow.xaml.cs
{
    /// <summary>
    /// Логика взаимодействия для PassGenWindow.xaml
    /// </summary>
    public partial class PassGenWindow : Window
    {
        public PassGenWindow()
========
{ 
    /// Логика взаимодействия для RegistrationPage.xaml
    public partial class RegistrationPage : UserControl
    {
        public RegistrationPage()

>>>>>>>> DbTest:mypass/View/RegistrationPage.xaml.cs
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
