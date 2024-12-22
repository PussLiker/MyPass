using mypass.Utilities;
using System;
using System.Windows.Input;

namespace mypass.ViewModel
{
    internal class MainAuthWindowVM : ViewModelBase
    {
        private object _pohujView = new VhodVM();
        private string _login;
        private string _password;
        private bool _isPasswordEmpty = true;

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
        public string Logintext
        {
            get => _login;
            set
            {
                if (_login != value)
                {
                    _login = value;
                    OnPropertyChanged(nameof(Logintext));
                }
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                if (value != _password)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }
        public ICommand RegPageCommand { get; set; }
        public ICommand LoginPageCommand { get; set; }

        public object PohujView
        {
            get { return _pohujView; }
            set { _pohujView = value; OnPropertyChanged(nameof(PohujView)); }
        }

        public void Registration(object obj) { PohujView = new RegistrationPageVM(); }
        public void Login(object obj) => PohujView = new VhodVM();

        public MainAuthWindowVM()
        {

            RegPageCommand = new RelayCommand(Registration);
            LoginPageCommand = new RelayCommand(Login);

        }
    }
}
