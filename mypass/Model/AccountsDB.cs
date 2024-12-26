using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace mypass.Model
{
    public class AccountsDB : DataBase
    {
        public AccountsDB(string databasePath)
        {
            _databasePath = databasePath;
            _connectionString = $"Data Source={_databasePath};Version=3;";
            _connection = new SQLiteConnection(_connectionString);
        }

        private int _idaccount;
        public int IdAccount
        {
            get => _idaccount;
            set => _idaccount = value;
        }

        private string _loginuseraccount;
        public string LoginUserAccount
        {
            get => _loginuseraccount;
            set => _loginuseraccount = value;
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

        private string _loginaccount;
        public string LoginAccount
        {
            get => _loginaccount;
            set => _loginaccount = value;
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => _password = value;
        }

        // Метод для добавления нового аккаунта
        public void AddAccount(string loginUserAccount, string serviceName, string url, string loginAccount, string password)
        {
            OpenConnection();
            string query = @"INSERT INTO Accounts (LoginUserAccount, ServiceName, URL, LoginAccount, Password) 
                             VALUES (@LoginUserAccount, @ServiceName, @URL, @LoginAccount, @Password);";

            using (var command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@LoginUserAccount", loginUserAccount);
                command.Parameters.AddWithValue("@ServiceName", serviceName);
                command.Parameters.AddWithValue("@URL", url);
                command.Parameters.AddWithValue("@LoginAccount", loginAccount);
                command.Parameters.AddWithValue("@Password", password);

                command.ExecuteNonQuery();
            }

            CloseConnection();
        }

        // Метод для обновления аккаунта
        public void UpdateAccount(int idAccount, string loginUserAccount, string serviceName, string url, string loginAccount, string password)
        {
            OpenConnection();

            string query = @"UPDATE Accounts SET 
                             LoginUserAccount = @LoginUserAccount,
                             ServiceName = @ServiceName, 
                             URL = @URL, 
                             LoginAccount = @LoginAccount, 
                             Password = @Password 
                             WHERE IdAccount = @IdAccount;";

            using (var command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@IdAccount", idAccount);
                command.Parameters.AddWithValue("@LoginUserAccount", loginUserAccount);
                command.Parameters.AddWithValue("@ServiceName", serviceName);
                command.Parameters.AddWithValue("@URL", url);
                command.Parameters.AddWithValue("@LoginAccount", loginAccount);
                command.Parameters.AddWithValue("@Password", password);

                command.ExecuteNonQuery();
            }

            CloseConnection();
        }

        // Метод для удаления аккаунта
        public void DeleteAccount(int idAccount)
        {
            OpenConnection();
            string query = "DELETE FROM Accounts WHERE IdAccount = @IdAccount;";

            using (var command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@IdAccount", idAccount);
                command.ExecuteNonQuery();
            }

            CloseConnection();
        }

        // Метод для получения всех данных об аккаунте в виде Dictionary<string, string>
        public Dictionary<string, string> GetAccountById(int idAccount)
        {
            OpenConnection();
            string query = "SELECT * FROM Accounts WHERE IdAccount = @IdAccount;";
            var accountData = new Dictionary<string, string>();

            using (var command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@IdAccount", idAccount);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        accountData["IdAccount"] = reader["IdAccount"].ToString();
                        accountData["LoginUserAccount"] = reader["LoginUserAccount"].ToString();
                        accountData["ServiceName"] = reader["ServiceName"].ToString();
                        accountData["URL"] = reader["URL"].ToString();
                        accountData["LoginAccount"] = reader["LoginAccount"].ToString();
                        accountData["Password"] = reader["Password"].ToString();
                    }
                }
            }

            CloseConnection();
            return accountData;
        }

        // Метод для загрузки всех аккаунтов из базы данных
        public List<Dictionary<string, string>> LoadDataFromAccountsDB()
        {
            OpenConnection();
            string query = "SELECT * FROM Accounts;";
            var accountsList = new List<Dictionary<string, string>>();

            using (var command = new SQLiteCommand(query, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var accountData = new Dictionary<string, string>
                        {
                            ["IdAccount"] = reader["IdAccount"].ToString(),
                            ["LoginUserAccount"] = reader["LoginUserAccount"].ToString(),
                            ["ServiceName"] = reader["ServiceName"].ToString(),
                            ["URL"] = reader["URL"].ToString(),
                            ["LoginAccount"] = reader["LoginAccount"].ToString(),
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
