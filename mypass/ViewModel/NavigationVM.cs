using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using mypass.Utilities;
using mypass.View;

namespace mypass.ViewModel
{
    class NavigationVM : ViewModelBase
    {
        private object _currentView;
        private Vhod _vhod;
        private MainWindow _mainWindow;
        private PassGenWindow _passGenWindow; // Поле для хранения ссылки на окно
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        public ICommand MinimizeCommand { get; }
        public ICommand MaximizeCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand AccountsCommand { get; set; }
        public ICommand AllPassesCommand { get; set; }
        public ICommand CardsCommand { get; set; }
        public ICommand EmailsCommand { get; set; }
        public ICommand MainMenuCommand { get; set; }
        public ICommand NotificationsCommand { get; set; }
        public ICommand PassCheckCommand { get; set; }
        public ICommand PassGenCommand { get; set; }
        public ICommand VhodStartCommand { get; set; }
        public ICommand VhodButtonCommand { get; set; }


        private void Accounts(object obj) => CurrentView = new AccountsVM();
        private void AllPasses(object obj) => CurrentView = new AllPassesVM();
        private void Cards(object obj) => CurrentView = new CardsVM();
        private void Emails(object obj) => CurrentView = new EmailsVM();
        private void MainMenu(object obj) => CurrentView = new MainMenuVM();
        private void Notifications(object obj) => CurrentView = new NofiticationsVM();
        private void PassCheck(object obj) => CurrentView = new PassCheckVM();
        private void VhodStart(object obj)
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
        
        private void PassGen(object obj)
        {
            if (_passGenWindow == null)
            {
                _passGenWindow = new PassGenWindow();
                _passGenWindow.Closed += (s, args) => _passGenWindow = null;
                _passGenWindow.Show();
            }
            else
            {
                _passGenWindow.Focus();
            }
        }
        private void Minimize(object obj)
        {

            if (obj is Window window)
            {
                window.WindowState = WindowState.Minimized;
            }
        }
        private void Maximize(object obj)
        {
            if (obj is Window window)
            {
                if (window.WindowState == WindowState.Maximized)
                {
                    window.WindowState = WindowState.Normal; // Восстановить окно
                }
                else
                {
                    window.WindowState = WindowState.Maximized; // Максимизировать окно
                }
            }
        }
        private void Close(object obj)
        {
            if (obj is Window window)
            { window?.Close(); }
        }

        public NavigationVM()
        {
            AccountsCommand = new RelayCommand(Accounts);
            CardsCommand = new RelayCommand(Cards);
            AllPassesCommand = new RelayCommand(AllPasses);
            EmailsCommand = new RelayCommand(Emails);
            MainMenuCommand = new RelayCommand(MainMenu);
            NotificationsCommand = new RelayCommand(Notifications);
            PassCheckCommand = new RelayCommand(PassCheck);
            PassGenCommand = new RelayCommand(PassGen);
            MinimizeCommand = new RelayCommand(Minimize);
            MaximizeCommand = new RelayCommand(Maximize);
            CloseCommand = new RelayCommand(Close);
            VhodStartCommand = new RelayCommand(VhodStart);
            //начальная страница
            CurrentView = new MainMenuVM();
        }
    }
}
