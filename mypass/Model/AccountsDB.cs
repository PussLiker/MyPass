using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace mypass.Model
{
    public class AccountsDB : DataBase
    {
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
        private string _servicename;
        public string ServiceName
        {
            get => _servicename;
            set => _servicename = value;
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

        // Метод для создания нового аккаунта
        public void AddAccount(int idAccount, string loginUserAccount, string serviceName, string url, string loginAccount, string password)
        {
            OpenConnection();
            string query = @"INSERT INTO Accounts (IdAccount, LoginUserAccount, ServiceName, URL, LoginAccount, Password) 
                             VALUES (@IdAccount, @LoginUserAccount, @ServiceName, @URL, @LoginAccount, @Password);";

            using (var command = new SqliteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@IdAccount", idAccount);
                command.Parameters.AddWithValue("@LoginUserAccount", loginUserAccount);
                command.Parameters.AddWithValue("@ServiceName", serviceName);
                command.Parameters.AddWithValue("@URL", url);
                command.Parameters.AddWithValue("@LoginAccount", loginAccount);
                command.Parameters.AddWithValue("@Password", password);

                int affectedRows = command.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    _idaccount = idAccount;
                    _loginuseraccount = loginUserAccount;
                    _servicename = serviceName;
                    _url = url;
                    _loginaccount = loginAccount;
                    _password = password;
                }
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

            using (var command = new SqliteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@IdAccount", idAccount);
                command.Parameters.AddWithValue("@LoginUserAccount", loginUserAccount);
                command.Parameters.AddWithValue("@ServiceName", serviceName);
                command.Parameters.AddWithValue("@URL", url);
                command.Parameters.AddWithValue("@LoginAccount", loginAccount);
                command.Parameters.AddWithValue("@Password", password);

                int affectedRows = command.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    _idaccount = idAccount;
                    _loginuseraccount = loginUserAccount;
                    _servicename = serviceName;
                    _url = url;
                    _loginaccount = loginAccount;
                    _password = password;
                }
            }
            CloseConnection();
        }

        // Метод для удаления аккаунта
        public void DeleteAccount(int idAccount)
        {
            OpenConnection();
            string query = "DELETE FROM Accounts WHERE IdAccount = @IdAccount;";

            using (var command = new SqliteCommand(query, _connection))
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

            using (var command = new SqliteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@IdAccount", idAccount);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        _idaccount = reader.GetInt32(reader.GetOrdinal("IdAccount"));
                        _loginuseraccount = reader["LoginUserAccount"].ToString();
                        _servicename = reader["ServiceName"].ToString();
                        _url = reader["URL"].ToString();
                        _loginaccount = reader["LoginAccount"].ToString();
                        _password = reader["Password"].ToString();

                        accountData["IdAccount"] = _idaccount.ToString();
                        accountData["LoginUserAccount"] = _loginuseraccount;
                        accountData["ServiceName"] = _servicename;
                        accountData["URL"] = _url;
                        accountData["LoginAccount"] = _loginaccount;
                        accountData["Password"] = _password;
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

            using (var command = new SqliteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@IdUser", userId);
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
