using System;
using System.Windows;
using System.Windows.Controls;

namespace mypass.Model
{
    public partial class BindablePasswordBoxNorm : UserControl
    {
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                "Password",
                typeof(string),
                typeof(BindablePasswordBoxNorm),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnPasswordPropertyChanged));

        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBoxNorm passwordBox)
            {
                if (passwordBox.passwordBox.Password != (string)e.NewValue)
                {
                    passwordBox.passwordBox.Password = (string)e.NewValue;
                }
            }
        }

        private bool _isUpdating;

        public BindablePasswordBoxNorm()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            
            if (!_isUpdating)
            {
                _isUpdating = true;
                Password = passwordBox.Password;
                _isUpdating = false;
            }       
        }
    }
}
    