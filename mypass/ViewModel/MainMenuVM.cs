using mypass.Model;
using mypass.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using static mypass.Model.PasswordValidator;

namespace mypass.ViewModel
{
    internal class MainMenuVM : Utilities.ViewModelBase
    {
        // Коллекции аккаунтов
        public ObservableCollection<Account> Accounts { get; set; } = new ObservableCollection<Account>();
        public ObservableCollection<Account> WeakPasswords { get; set; } = new ObservableCollection<Account>();
        public ObservableCollection<Account> StrongPasswords { get; set; } = new ObservableCollection<Account>();
        public ObservableCollection<Account> DuplicatePasswords { get; set; } = new ObservableCollection<Account>();

        private int _weakPasswordsCount = 0;
        private int _strongPasswordsCount = 0;
        private int _duplicatePasswordsCount = 0;

        public int WeakPasswordsCount
        {
            get => _weakPasswordsCount;
            set
            {
                _weakPasswordsCount = value;
                OnPropertyChanged(nameof(WeakPasswordsCount));
            }
        }

        public int StrongPasswordsCount
        {
            get => _strongPasswordsCount;
            set
            {
                _strongPasswordsCount = value;
                OnPropertyChanged(nameof(StrongPasswordsCount));
            }
        }

        public int DuplicatePasswordsCount
        {
            get => _duplicatePasswordsCount;
            set
            {
                _duplicatePasswordsCount = value;
                OnPropertyChanged(nameof(DuplicatePasswordsCount));
            }
        }

        private AccountsDB _accountsDB;

        public MainMenuVM()
        {
            MainAuthWindowVM mainAuthWindowVM = new MainAuthWindowVM();
            _accountsDB = new AccountsDB(mainAuthWindowVM.FullBDPath);

            // Загрузка данных из базы
            LoadAccounts();
        }

        private void LoadAccounts()
        {
            var accountsFromDb = _accountsDB.LoadDataFromAccountsDB();
            Accounts.Clear();

            foreach (var accountData in accountsFromDb)
            {
                var account = new Account
                {
                    Username = accountData["ServiceName"],
                    Email = accountData["LoginAccount"],
                    Password = EncryptionModel.Decrypt(accountData["Password"], PageModel.masterPassword),
                    URL = accountData["URL"]
                };
                Accounts.Add(account);
            }

            // Обновляем классификации
            ClassifyPasswords();
        }

        private void ClassifyPasswords()
        {
            // Очищаем предыдущие списки
            WeakPasswords.Clear();
            StrongPasswords.Clear();
            DuplicatePasswords.Clear();

            // Классификация паролей
            var passwordsGrouped = Accounts.GroupBy(a => a.Password).Where(g => g.Count() > 1);

            foreach (var account in Accounts)
            {
                var (strength, feedback) = CheckPasswordStrength(account.Password);

                if (strength == PasswordStrength.VeryWeak || strength == PasswordStrength.Weak)
                {
                    WeakPasswords.Add(account);
                }
                else
                {
                    StrongPasswords.Add(account);
                }

                if (passwordsGrouped.Any(g => g.Key == account.Password))
                {
                    DuplicatePasswords.Add(account);
                }
            }

            // Обновляем счетчики
            WeakPasswordsCount = WeakPasswords.Count;
            StrongPasswordsCount = StrongPasswords.Count;
            DuplicatePasswordsCount = DuplicatePasswords.Count;
        }
    }
}
