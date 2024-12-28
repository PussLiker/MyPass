using mypass.Model;
using mypass.Utilities;
using mypass.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public ICommand AddAccountCommand { get; set; }
        public ICommand EditAccountCommand { get; set; }
        public ICommand DeleteAccountCommand { get; set; }
        public ICommand ShowAccountMenuCommand { get; set; }
        public ICommand OpenAddAccountWindowCommand { get; set; }
        public ICommand OpenEditAccauntWindowCommand { get; set; }

        public ObservableCollection<Account> AccountsCollection { get; set; }

        private AccountsDB _accountsDB; // База данных для работы с аккаунтами

        public AllPassesVM()
        {
            MainAuthWindowVM mainAuthWindowVM = new MainAuthWindowVM();
            // Инициализация базы данных
            _accountsDB = new AccountsDB(mainAuthWindowVM.FullBDPath);

            // Привязка команд
            CopyEmailCommand = new RelayCommand(CopyEmail);
            CopyPasswordCommand = new RelayCommand(CopyPassword);
            OpenAddAccountWindowCommand = new RelayCommand(OpenAddAccountWindow);
            OpenEditAccauntWindowCommand = new RelayCommand(OpenEditAccauntWindow);

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

            DeleteAccountCommand = new RelayCommand(DeleteAccount);
            // Инициализация коллекции аккаунтов
            AccountsCollection = new ObservableCollection<Account>();

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
                    ID = int.Parse(accountData["IdAccount"]),
                    Username = accountData["ServiceName"],
                    Email = accountData["LoginAccount"],
                    Password = EncryptionModel.Decrypt(accountData["Password"], PageModel.masterPassword),
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

        private void OpenAddAccountWindow(object parameter)
        {
            // Создаем экземпляр окна
            var addAccountWindow = new AddAccountWindow();  // Убедитесь, что AddAccountWindow.xaml.cs существует

            // Открываем окно модально
            addAccountWindow.ShowDialog();  // Показываем окно модально

            // После того как окно закрыто, обновляем список аккаунтов
            LoadAccounts();
        }

        private void OpenEditAccauntWindow(object parameter)
        {
            if (parameter is Account account)
            {
                var editAccauntWindow = new EditAccountWindow();
                PageModel.ID = account.ID;
                editAccauntWindow.ShowDialog();
                LoadAccounts();
            }
        }


        // Удаление аккаунта
        private void DeleteAccount(object parameter)
        {
            if (parameter is Account account)
            {
                _accountsDB.DeleteAccount(account.ID);
                LoadAccounts();
            }
        }
    }
}
