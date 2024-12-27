using mypass.Utilities;
using System;
using System.Windows.Input;
using mypass.Model;
using System.Windows;
using mypass.View;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.Common;

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
        private MainWindow _mainWindow;
        private static string _fullBDPath;
        private string _bdName;
        RegistrationPage _registrationPage ;
        MainAuthWindowClass MainWinClass = new MainAuthWindowClass();
        public string DBName { get => _bdName; set { _bdName = value; } }
        public string PassReg
        {
            get => _passReg;
            set
            {
                        if (value != _passRegConf)
                        {
                            _passReg = value;
                            OnPropertyChanged(nameof(PassReg));
                        }
            }
        }
        public string FullBDPath
        {
            get => _fullBDPath;
            set { _fullBDPath = value; }
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

        public void SwitchWindow(object obj)
        {
            if (obj is Window window)
            {
                if (_mainWindow == null)
                {
                    _mainWindow = new MainWindow();
                    _mainWindow.Closed += (s, args) => _mainWindow = null;
                    _mainWindow.Show();
                }
                else
                    _mainWindow = new MainWindow();
                window?.Close();
            }
        }
    

        public ICommand RegPageCommand { get; set; }
        public ICommand LoginPageCommand { get; set; }
        public ICommand RegistrationToSystemCommand {  get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand VhodimCommand { get; set; }
        public object PohujView
        {
            get { return _pohujView; }
            set { _pohujView = value; OnPropertyChanged(nameof(PohujView)); }
        }

        public void Registration(object obj) { PohujView = new RegistrationPageVM(); }
        public void Login(object obj) => PohujView = new VhodVM();
        public void RegistrationToSystem(object obj) {
            if (MainWinClass.GdePole(LoginReg) && MainWinClass.GdePole(Username) && MainWinClass.GdePole(UserSecondname))
            {
                switch (MainWinClass.IsPasswordNorm(PassReg, PassRegConf))
                {
                    case 1:
                        MainWinClass.Registration(LoginReg, PassRegConf, Username, UserSecondname);
                        SwitchWindow(obj);
                        break;
                    case 2:
                        MainWinClass.PassNotEquel();
                        break;
                    case 3:
                        MainWinClass.PassNotNull();
                        break;
                }
            }
            else 
            {
            MainWinClass.GdePolaNull();
            }
        }
        public void Exit(object obj)
        {
            if (obj is Window w)
            { w?.Close(); }
        }

        public void Vhodim(object obj)
        {
            if (Logintext != null && Password != null) {
                if(MainWinClass.Vhodim(Logintext, Password)){
                    _bdName = $"{Logintext}.db";
                    FullBDPath = Path.Combine(MainWinClass.DBpath,DBName);
                    SwitchWindow(obj);
                }
            }
            else if (Logintext == null && Password == null)
            {
                ErrorWindow.ShowError("Вы не ввели данные!");
            }
            else if(Logintext == null)
            {
                ErrorWindow.ShowError("Логин не может быть пустым!");
                Logintext = null;
                Password = null;
            }
            else
            {
                ErrorWindow.ShowError("Пароль не может быть пустым!");
                Logintext = null;
                Password = null;
            }
        }

        public MainAuthWindowVM()
        {
            ExitCommand = new RelayCommand(Exit);
            RegPageCommand = new RelayCommand(Registration);
            LoginPageCommand = new RelayCommand(Login);
            RegistrationToSystemCommand = new RelayCommand(RegistrationToSystem);
            VhodimCommand = new RelayCommand(Vhodim);
        }
    }
}
