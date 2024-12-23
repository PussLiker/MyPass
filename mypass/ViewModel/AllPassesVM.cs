using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using mypass.Model;
using mypass.Utilities;

namespace mypass.ViewModel
{
    internal class AllPassesVM : Utilities.ViewModelBase
    {
        public ObservableCollection<Account> Accounts { get; set; }
        public ICommand OpenLinkCommand { get; }
        public ICommand CopyEmailCommand { get; }
        public ICommand CopyPasswordCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }

        public AllPassesVM()
        {
            // Привязываем команды к методам с учетом параметров
            CopyEmailCommand = new RelayCommand(CopyEmail);
            CopyPasswordCommand = new RelayCommand(CopyPassword);
            TogglePasswordVisibilityCommand = new RelayCommand(TogglePasswordVisibility); // Используем RelayCommand без параметра типа

            OpenLinkCommand = new RelayCommand(parameter =>
            {
                if (parameter is Account account)
                {
                    account.OpenLink();
                }
            });

            Accounts = new ObservableCollection<Account>
            {
                new Account { Username = "vk.com", Email = "user1@example.com", Password = "password" },
                new Account { Username = "not-a-url.ru", Email = "user2@example.com", Password = "password" },
                new Account { Username = "", Email = "user1@example.com", Password = "password" },
                new Account { Username = "https://not-a-url", Email = "user2@example.com", Password = "password" },
                new Account { Username = "http://pornhub.com", Email = "user1@example.com", Password = "password" },
                new Account { Username = "ghsd;l'kjsdfghladfgb;ljh", Email = "user2@example.com", Password = "password" }
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

        private void TogglePasswordVisibility(object parameter)
        {
            if (parameter is Account account)
            {
                account.IsPasswordVisible = !account.IsPasswordVisible; // Переключаем видимость пароля для конкретной учетной записи
            }
        }
    }
}
