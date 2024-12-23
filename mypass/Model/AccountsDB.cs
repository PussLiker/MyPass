using System;
using System.Collections.Generic;
using System.Data;

using System.Data.SQLite;
using System.Security.Policy;

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

        // Метод для создания нового аккаунта
        public void AddAccount(int IdAccount, string LoginUserAccount, string ServiceName, string URL, string LoginAccount, string Password)
        {
            OpenConnection();
            int affectedRows;
            string query = @"INSERT INTO Accounts (IdAccount, LoginUserAccount, ServiceName, URL, LoginAccount, Password) 
                             VALUES (@IdAccount, @LoginUserAccount, @ServiceName, @URL, @LoginAccount, @Password);";

            using (var command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@IdAccount", IdAccount);
                command.Parameters.AddWithValue("@LoginUserAccount", LoginUserAccount);
                command.Parameters.AddWithValue("@ServiceName", ServiceName);
                command.Parameters.AddWithValue("@URL", URL);
                command.Parameters.AddWithValue("@LoginAccount", LoginAccount);
                command.Parameters.AddWithValue("@Password", Password);

                affectedRows = command.ExecuteNonQuery();
            }
            CloseConnection();

            // Обновляем поля класса, если обновление прошло успешно
            if (affectedRows > 0) 
            {
                _idaccount = IdAccount;
                _loginuseraccount = LoginUserAccount;
                _servisename = ServiceName;
                _url = URL;
                _loginaccount = LoginAccount;
                _password = Password;
            }
        }

        // Метод для обновления аккаунта
        public void UpdateAccount(int IdAccount, string LoginUserAccount, string ServiceName, string URL, string LoginAccount, string Password)
        {
            OpenConnection();

            int affectedRows;

            string query = @"UPDATE Accounts SET 
                             LoginUserAccount = @LoginUserAccount,
                             ServiceName = @ServiceName, 
                             URL = @URL, 
                             LoginAccount = @LoginAccount, 
                             Password = @Password 
                             WHERE IdAccount = @IdAccount;";

            using (var command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@IdAccount", IdAccount);
                command.Parameters.AddWithValue("@LoginUserAccount", LoginUserAccount);
                command.Parameters.AddWithValue("@ServiceName", ServiceName);
                command.Parameters.AddWithValue("@URL", URL);
                command.Parameters.AddWithValue("@Login", LoginAccount);
                command.Parameters.AddWithValue("@Password", Password);

                affectedRows = command.ExecuteNonQuery();
            }
            CloseConnection();

            // Обновляем поля класса, если обновление прошло успешно
            if (affectedRows > 0)
            {
                _idaccount = IdAccount;
                _loginuseraccount = LoginUserAccount;
                _servisename = ServiceName;
                _url = URL;
                _loginaccount = LoginAccount;
                _password = Password;
            }
        }

        // Метод для удаления аккаунта
        public void DeleteAccount(int IdAccount)
        {
            OpenConnection();
            int affectedRows;
            string query = "DELETE FROM Accounts WHERE IdAccount = @IdAccount;";

            using (var command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@IdAccount", IdAccount);
                affectedRows = command.ExecuteNonQuery();
            }
            CloseConnection();

            // Обновляем поля класса, если обновление прошло успешно
            if (affectedRows > 0)
            {
                _idaccount = IdAccount;
                _loginuseraccount = "";
                _servisename = "";
                _url = "";
                _loginaccount = "";
                _password = "";
            }
        }

        // Метод для получения всех данных об аккаунте в виде Dictionary<string, string>
        public Dictionary<string, string> GetAccountById(int IdAccount)
        {
            OpenConnection();
            string query = "SELECT * FROM Accounts WHERE IdAccount = @IdAccount;";
            var accountData = new Dictionary<string, string>();

            using (var command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@IdAccount", IdAccount);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        _idaccount = Convert.ToInt32(reader["IdTag"]);
                        _loginuseraccount = reader["LoginUserAccount"].ToString();
                        _servisename = reader["ServiceName"].ToString();
                        _url = reader["URL"].ToString();
                        _loginaccount = reader["LoginAccount"].ToString();
                        _password = reader["Password"].ToString();

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
        public void LoadDataFromAccountsDB()
        {
            OpenConnection();

            string query = "SELECT LoginUser, FirstName, SecondName, MasterPasswordHash, Salt FROM User;";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _idaccount = Convert.ToInt32(reader["IdTag"]);
                        _loginuseraccount = reader["@LoginUserAccount"].ToString();
                        _servisename = reader["@ServiceName"].ToString();
                        _url = reader["@URL"].ToString();
                        _loginaccount = reader["@LoginAccount"].ToString();
                        _password = reader["@Password"].ToString();
                    }
                }
            }
            CloseConnection();
        }
    }
}
