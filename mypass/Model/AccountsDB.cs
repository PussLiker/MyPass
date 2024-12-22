using System.Collections.Generic;

using System.Data.SQLite;

namespace mypass.Model
{
    public class AccountsDB : DataBase
    {
        private int _idaccount;
        public int IDAccount
        {
            get => _idaccount;
            set => _idaccount = value;
        }
        private int _iduser;
        public int IDUser
        {
            get => _iduser;
            set => _iduser = value;
        }
        private string _servisename;
        public string ServiseName
        {
            get => _servisename;
            set => _servisename = value;
        }
        private string _url;
        public string Url
        {
            get => _url;
            set => _url = value;
        }
        private string _login;
        public string Login
        {
            get => _login;
            set => _login = value;
        }
        private string _password;
        public string Password
        {
            get => _password;
            set => _password = value;
        }

        // Инициализация конструктора
        public AccountsDB(string databasePath, string password) : base() { }

        // Метод для создания нового аккаунта
        public void AddAccount(int userId, string serviceName, string url, string login, string password)
        {
            OpenConnection();
            string query = @"INSERT INTO Accounts (IdUser, ServiceName, URL, Login, Password) 
                             VALUES (@IdUser, @ServiceName, @URL, @Login, @Password);";

            using (var command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@IdUser", userId);
                command.Parameters.AddWithValue("@ServiceName", serviceName);
                command.Parameters.AddWithValue("@URL", url);
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Password", password);

                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        // Метод для обновления аккаунта
        public void UpdateAccount(int accountId, string serviceName, string url, string login, string password)
        {
            OpenConnection();
            string query = @"UPDATE Accounts SET 
                             ServiceName = @ServiceName, 
                             URL = @URL, 
                             Login = @Login, 
                             Password = @Password 
                             WHERE IdAccount = @IdAccount;";

            using (var command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@IdAccount", accountId);
                command.Parameters.AddWithValue("@ServiceName", serviceName);
                command.Parameters.AddWithValue("@URL", url);
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Password", password);

                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        // Метод для удаления аккаунта
        public void DeleteAccount(int accountId)
        {
            OpenConnection();
            string query = "DELETE FROM Accounts WHERE IdAccount = @IdAccount;";

            using (var command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@IdAccount", accountId);
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        // Метод для получения всех данных об аккаунте в виде Dictionary<string, string>
        public Dictionary<string, string> GetAccountById(int accountId)
        {
            OpenConnection();
            string query = "SELECT * FROM Accounts WHERE IdAccount = @IdAccount;";
            var accountData = new Dictionary<string, string>();

            using (var command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@IdAccount", accountId);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        accountData["IdAccount"] = reader["IdAccount"].ToString();
                        accountData["IdUser"] = reader["IdUser"].ToString();
                        accountData["ServiceName"] = reader["ServiceName"].ToString();
                        accountData["URL"] = reader["URL"].ToString();
                        accountData["Login"] = reader["Login"].ToString();
                        accountData["Password"] = reader["Password"].ToString();
                    }
                }
            }

            CloseConnection();
            return accountData;
        }

        // Метод для получения списка всех аккаунтов пользователя
        public List<Dictionary<string, string>> GetAllAccountsByUserId(int userId)
        {
            OpenConnection();
            string query = "SELECT * FROM Accounts WHERE IdUser = @IdUser;";
            var accountsList = new List<Dictionary<string, string>>();

            using (var command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@IdUser", userId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var accountData = new Dictionary<string, string>
                        {
                            ["IdAccount"] = reader["IdAccount"].ToString(),
                            ["IdUser"] = reader["IdUser"].ToString(),
                            ["ServiceName"] = reader["ServiceName"].ToString(),
                            ["URL"] = reader["URL"].ToString(),
                            ["Login"] = reader["Login"].ToString(),
                            ["Password"] = reader["Password"].ToString()
                        };
                        accountsList.Add(accountData);
                    }
                }
            }

            CloseConnection();
            return accountsList;
        }
    }
}
