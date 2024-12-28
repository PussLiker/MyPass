using mypass.Model;
using mypass.Utilities;
using mypass.View;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace mypass.ViewModel
{
    internal class NewAccountVM : Utilities.ViewModelBase
    {
        MainAuthWindowVM mainAuthWindowVM = new MainAuthWindowVM();
        public string ServiceName { get; set; }
        public string URL { get; set; }
        public string LoginAccount { get; set; }
        public string Password { get; set; }
        public string NewServiceName { get; set; }
        public string NewURL { get; set; }
        public string NewLoginAccount { get; set; }
        public string NewPassword { get; set; }
        private static int _id;
        
        public int ID { get => _id; set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged(nameof(ID));
                }
            }
        }
        public ICommand EditAccountCommand { get; set; }
        public ICommand AddAccountCommand { get; }
        public ICommand CloseCommand { get; }

        private AccountsDB _accountsDB; // База данных для работы с аккаунтами
        private string _loginUserAccount;

        public NewAccountVM()
        {
            // Пример автоматического получения логина пользователя
            _loginUserAccount = PageModel.login; 

            AddAccountCommand = new RelayCommand(AddAccount);
            CloseCommand = new RelayCommand(Close);
            EditAccountCommand = new RelayCommand(EditAccount);
            _accountsDB = new AccountsDB(mainAuthWindowVM.FullBDPath);  // Укажите путь к вашей базе данных

            AddTagCommand = new RelayCommand(AddTag);   

        }

        private void AddAccount(object parameter)
        {
            if (string.IsNullOrEmpty(ServiceName) || string.IsNullOrEmpty(URL) || string.IsNullOrEmpty(Password))
            {
                // Убедитесь, что все поля заполнены
                return;
            }

            // Добавление аккаунта в базу данных
            _accountsDB.AddAccount(_loginUserAccount, ServiceName, URL, LoginAccount, EncryptionModel.Enscrypt(Password, PageModel.masterPassword));

            // Закрытие окна после добавления записи
            // Предполагается, что окно будет закрываться из кода
            var window = parameter as Window;
            if (window != null)
            {
                window.Close();
            }
        }

        private void EditAccount(object parameter)
        {
            if (string.IsNullOrEmpty(NewServiceName) || string.IsNullOrEmpty(NewURL) || string.IsNullOrEmpty(NewPassword))
            {
                // Убедитесь, что все поля заполнены
                return;
            }
            AllPassesVM allPassesVM = new AllPassesVM();
                _accountsDB.UpdateAccount(PageModel.ID, _loginUserAccount, NewServiceName, NewURL, NewLoginAccount, EncryptionModel.Enscrypt(NewPassword, PageModel.masterPassword));


            

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



        //__________________________________________________________________________________________________________________________________
        //РЕАЛИЗАЦИЯ БЕК + БД ДЛЯ ТЕГОВ, ЗАКОММЕНТИТЬ В СЛУЧАЕ ЧП
        //__________________________________________________________________________________________________________________________________

        public ICommand AddTagCommand { get; set; }
        public ICommand ChangeTag {  get; set; }
        private string _tagName;
        
        public string TagName
        {
            get => _tagName;
            set
            {
                if (value != _tagName)
                {
                    _tagName = value;
                    OnPropertyChanged(nameof(_tagName));
                }
            }
        }

        private void AddTag(object obj)
        {
            TagsDB tagsDB = new TagsDB(mainAuthWindowVM.DBName);
            if (tagsDB != null)
            {
                tagsDB.AddTag(_tagName);
            }
            else
            {
                ErrorWindow.ShowError("Тег не может быть пустым!");
            }
        }


    }
}
