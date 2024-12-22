using mypass.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mypass.Model;
using System.Windows.Input;
using mypass.Utilities;
using mypass.View;
using System.Windows;

namespace mypass.ViewModel
{
    internal class AllPassesVM : Utilities.ViewModelBase
    {

        public ObservableCollection<Account> Accounts { get; set; }
        public ICommand OpenLinkCommand { get; }
        // Команда для копирования Email
        public ICommand CopyEmailCommand { get; }
        // Команда для копирования Password
        public ICommand CopyPasswordCommand { get; }
        public AllPassesVM()
        {
            CopyEmailCommand = new RelayCommand(CopyEmail);
            CopyPasswordCommand = new RelayCommand(CopyPassword);

            OpenLinkCommand = new RelayCommand(parameter =>
            {
                if (parameter is Account account)
                {
                    account.OpenLink();
                }
            });

            Accounts = new ObservableCollection<Account>
            {
                new Account { Username = "vk.com", Email = "user1@example.com", Password = "password"},
                new Account { Username = "not-a-url.ru", Email = "user2@example.com", Password = "password"},
                new Account { Username = "", Email = "user1@example.com", Password = "password"},
                new Account { Username = "https://not-a-url", Email = "user2@example.com", Password = "password"},
                new Account { Username = "http://pornhub.com", Email = "user1@example.com", Password = "password"},
                new Account { Username = "ghsd;l'kjsdfghladfgb;ljh", Email = "user2@example.com", Password = "password"},
            };
        }
        private void CopyEmail(object parameter)
        {
            if (parameter is Account account)
            {
                Clipboard.SetText(account.Email); // Копирует Email в буфер обмена
            }
        }

        private void CopyPassword(object parameter)
        {
            if (parameter is Account account)
            {
                Clipboard.SetText(account.Password); // Копирует Password в буфер обмена
            }
        }
    }
}
