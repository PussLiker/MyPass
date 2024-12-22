using mypass.Utilities;
using System;
using System.Windows.Input;
using mypass.Model;

namespace mypass.ViewModel
{
    internal class MainAuthWindowVM : ViewModelBase
    {
        private string _username;
        private string _userSecondName;
        private object _pohujView = new VhodVM();
        private string _login;
        private string _password;
        private string _passReg;
        private string _passRegConf;
        private string _loginReg;
        private bool _isPasswordEmpty = true;

        MainAuthWindow MainWinClass = new MainAuthWindow();
        public string PassReg
        {
            get => _passReg;
            set
            {               
                    _passReg = value;
                    OnPropertyChanged(nameof(PassReg));                
            }
        }
        public string PassRegConf
        {
            get => _passRegConf;

            set
            {
                if (value != _passRegConf)
                {
                    _passRegConf = value;
                    OnPropertyChanged(nameof(PassRegConf));
                }
            }
        }
        public string LoginReg
        {
            get => _loginReg;

            set 
            { 
                if(value != _loginReg)
                {
                _loginReg = value;
                OnPropertyChanged(nameof(LoginReg));        
                } 
            }

        }
        public string Username
        {
           get => _username;

           set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }
        public string UserSecondname
        {
            get => _userSecondName;

            set
            {
                if (value != _userSecondName)
                {
                    _userSecondName = value;
                    OnPropertyChanged(nameof(UserSecondname));
                }
            }
        }
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
        public ICommand RegistrationToSystemCommand {  get; set; }
        public object PohujView
        {
            get { return _pohujView; }
            set { _pohujView = value; OnPropertyChanged(nameof(PohujView)); }
        }

        public void Registration(object obj) { PohujView = new RegistrationPageVM(); }
        public void Login(object obj) => PohujView = new VhodVM();
        public void RegistrationToSystem(object obj) { MainWinClass.Registration(LoginReg, PassRegConf, Username, UserSecondname); }
        

        public MainAuthWindowVM()
        {

            RegPageCommand = new RelayCommand(Registration);
            LoginPageCommand = new RelayCommand(Login);
            RegistrationToSystemCommand = new RelayCommand(RegistrationToSystem);
        }
    }
}
