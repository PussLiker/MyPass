using System.Collections.Generic;

namespace mypass.Model
{
    internal class PasswordDoublicateChecker
    {
        private readonly AccountsDB _accountsDB;

        // Конструктор принимает объект AccountsDB
        public PasswordDoublicateChecker(AccountsDB accountsDB)
        {
            _accountsDB = accountsDB;
        }

        // Метод для проверки повторяющихся паролей у одного пользователя
        public List<Dictionary<string, string>> CheckForDuplicatePasswords(int userId)
        {
            var duplicateAccounts = new List<Dictionary<string, string>>();
            var passwordDictionary = new Dictionary<string, List<int>>();  // Словарь для хранения паролей и их Id

            // Получаем все аккаунты пользователя
            var accounts = _accountsDB.GetAllAccountsByUserId(userId);

            // Заполняем словарь с паролями
            foreach (var account in accounts)
            {
                int accountId = int.Parse(account["IdAccount"]);
                string password = account["Password"];
                string serviceName = account["ServiceName"];

                if (passwordDictionary.ContainsKey(password))
                {
                    passwordDictionary[password].Add(accountId);
                }
                else
                {
                    passwordDictionary[password] = new List<int> { accountId };
                }
            }

            // Проверяем, какие пароли используются в нескольких аккаунтах
            foreach (var entry in passwordDictionary)
            {
                if (entry.Value.Count > 1) // Если пароли повторяются
                {
                    foreach (var accountId in entry.Value)
                    {
                        var duplicateAccount = _accountsDB.GetAccountById(accountId);
                        duplicateAccounts.Add(duplicateAccount);
                    }
                }
            }

            return duplicateAccounts;
        }
    }
}
