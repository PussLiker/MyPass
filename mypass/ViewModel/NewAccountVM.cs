using mypass.Model;
using mypass.Utilities;
using System.Windows;
using System.Windows.Input;

namespace mypass.ViewModel
{
    internal class NewAccountVM : Utilities.ViewModelBase
    {
        public string ServiceName { get; set; }
        public string URL { get; set; }
        public string LoginAccount { get; set; }
        public string Password { get; set; }
        public ICommand AddAccountCommand { get; }
        public ICommand CloseCommand {  get; }

        private AccountsDB _accountsDB; // База данных для работы с аккаунтами
        private string _loginUserAccount;

        public NewAccountVM()
        {
            // Пример автоматического получения логина пользователя
            _loginUserAccount = "user1"; // Здесь будет логика для получения текущего логина пользователя

            AddAccountCommand = new RelayCommand(AddAccount);
            CloseCommand = new RelayCommand(Close);
            _accountsDB = new AccountsDB("D:\\App\\mypass\\bin\\Debug\\DataBase\\yadernijhuesos.db");  // Укажите путь к вашей базе данных
        }

        private void AddAccount(object parameter)
        {
            if (string.IsNullOrEmpty(ServiceName) || string.IsNullOrEmpty(URL) || string.IsNullOrEmpty(Password))
            {
                // Убедитесь, что все поля заполнены
                return;
            }

            // Добавление аккаунта в базу данных
            _accountsDB.AddAccount(_loginUserAccount, ServiceName, URL, LoginAccount, EncryptionModel.Enscrypt(Password, "123"));

            // Закрытие окна после добавления записи
            // Предполагается, что окно будет закрываться из кода
            var window = parameter as Window;
            if (window != null)
            {
                window.Close();
            }
        }
        private void Close(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }
    }
}
