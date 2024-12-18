using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mypass.Utilities;
using mypass.Model;
using mypass.View;
using System.Windows.Input;
using System.Windows.Controls;

namespace mypass.ViewModel
{
    internal class MainAuthWindowVM : ViewModelBase
    {
        private object _pohujView;
        private string _login;
        private string _password;
        

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
            set { _pohujView = value; OnPropertyChanged();}
        }

        public void Registration(object obj) => PohujView = new RegistrationPageVM();
        public void Login(object obj) => PohujView = new VhodVM();

        public MainAuthWindowVM()
        {
            PohujView = new VhodVM();

            RegPageCommand = new RelayCommand(Registration);
            LoginPageCommand = new RelayCommand(Login);


        }
    }
}
