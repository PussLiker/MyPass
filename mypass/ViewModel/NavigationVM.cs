using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using mypass.Utilities;
using mypass.View;

namespace mypass.ViewModel
{
    class NavigationVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand AccountsCommand { get; set; }
        public ICommand AllPassesCommand { get; set; }
        public ICommand CardsCommand { get; set; }
        public ICommand EmailsCommand { get; set; }
        public ICommand MainMenuCommand { get; set; }
        public ICommand NotificationsCommand { get; set; }
        public ICommand PassCheckCommand { get; set; }
        public ICommand PassGenCommand { get; set; }

        private void Accounts(object obj) => CurrentView = new AccountsVM();
        private void AllPasses(object obj) => CurrentView = new AllPassesVM();
        private void Cards(object obj) => CurrentView = new CardsVM();
        private void Emails(object obj) => CurrentView = new EmailsVM();
        private void MainMenu(object obj) => CurrentView = new MainMenuVM();
        private void Notifications(object obj) => CurrentView = new NofiticationsVM();
        private void PassCheck(object obj) => CurrentView = new PassCheckVM();
        private void PassGen(object obj) {
            PassGenWindow passGenWindow = new PassGenWindow();
            passGenWindow.Show();
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

            //начальная страница
            CurrentView = new MainMenuVM();
        }
    }
}
