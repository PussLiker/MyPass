using mypass.Model;
using mypass.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace mypass.ViewModel
{
    internal class AllPassesVM : Utilities.ViewModelBase
    {
        // Инициализируем коллекцию Accounts при объявлении
        public ObservableCollection<Account> Accounts { get; set; } = new ObservableCollection<Account>();

        public ICommand OpenLinkCommand { get; }
        public ICommand CopyEmailCommand { get; }
        public ICommand CopyPasswordCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }
        public ICommand OpenAddAccountWindowCommand { get; }

        private AccountsDB _accountsDB; // База данных для работы с аккаунтами

        public AllPassesVM()
        {
            //OpenAddAccountWindowCommand = new RelayCommand(OpenAddAccountWindow);

            // Инициализация базы данных
            _accountsDB = new AccountsDB("D:\\App\\mypass\\bin\\Debug\\DataBase\\yadernijhuesos.db");  // Замените на путь к вашей базе данных

            // Привязка команд
            CopyEmailCommand = new RelayCommand(CopyEmail);
            CopyPasswordCommand = new RelayCommand(CopyPassword);
            TogglePasswordVisibilityCommand = new RelayCommand(TogglePasswordVisibility);

            // Команда для открытия ссылки
            OpenLinkCommand = new RelayCommand(parameter =>
            {
                if (parameter is Account account)
                {
                    OpenUrl(account.URL);
                }
            });

            // Загружаем данные из базы
            LoadAccounts();
        }

        private void LoadAccounts()
        {
            // Загружаем все аккаунты из базы данных и обновляем коллекцию
            var accountsFromDb = _accountsDB.LoadDataFromAccountsDB();
            Accounts.Clear();

            foreach (var accountData in accountsFromDb)
            {
                Accounts.Add(new Account
                {
                    Username = accountData["ServiceName"],
                    Email = accountData["LoginAccount"],
                    Password = EncryptionModel.Decrypt(accountData["Password"], "123"),
                    URL = accountData["URL"]
                });
            }
        }

        private void CopyEmail(object parameter)
        {
            if (parameter is Account account)
            {
                Clipboard.SetText(account.Email);
            }
        }

        private void CopyPassword(object parameter)
        {
            if (parameter is Account account)
            {
                Clipboard.SetText(account.Password);
            }
        }

        private void TogglePasswordVisibility(object parameter)
        {
            if (parameter is Account account)
            {
                account.IsPasswordVisible = !account.IsPasswordVisible;
            }
        }

        // Метод для открытия URL
        private void OpenUrl(string url)
        {
            OpenLink.Open(url);
        }
        
    }
}
