using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using mypass.Utilities;
using mypass.View;
using mypass.Model;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace mypass.ViewModel
{
    class NavigationVM : ViewModelBase
    {
        private object _currentView;
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
        private void MainMenu(object obj) => CurrentView = new MainMenuVM();
        private void Notifications(object obj) => CurrentView = new NofiticationsVM();
        private void PassCheck(object obj) => CurrentView = new PassCheckVM();

        
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
            ((Window)obj).Close();
        }

        public NavigationVM()
        {
            AccountsCommand = new RelayCommand(Accounts);
            AllPassesCommand = new RelayCommand(AllPasses);
            MainMenuCommand = new RelayCommand(MainMenu);
            NotificationsCommand = new RelayCommand(Notifications);
            PassCheckCommand = new RelayCommand(PassCheck);
            PassGenCommand = new RelayCommand(PassGen);
            MinimizeCommand = new RelayCommand(Minimize);
            MaximizeCommand = new RelayCommand(Maximize);
            CloseCommand = new RelayCommand(Close);
            //начальная страница
            CurrentView = new MainMenuVM();
        }
    }
}
