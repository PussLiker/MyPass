using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Web.UI.WebControls;

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

        private string _loginuser;
        public string LoginUserAccount
        {
            get => _loginuser;
            set => _loginuser = value;
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

        // Метод для добавления нового аккаунта
        public void AddAccount(string loginUser, string serviceName, string url, string loginAccount, string password)
        {
            OpenConnection();
            string query = @"INSERT INTO Accounts (LoginUser, ServiceName, URL, LoginAccount, Password) 
                             VALUES (@LoginUser, @ServiceName, @URL, @LoginAccount, @Password);";
            int affectedRows;

            using (var command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@LoginUser", loginUser);
                command.Parameters.AddWithValue("@ServiceName", serviceName);
                command.Parameters.AddWithValue("@URL", url);
                command.Parameters.AddWithValue("@LoginAccount", loginAccount);
                command.Parameters.AddWithValue("@Password", password);

                affectedRows = command.ExecuteNonQuery();
            }

            long idacount = _connection.LastInsertRowId;

            CloseConnection();

            // Обновляем поля класса, если обновление прошло успешно
            if (affectedRows > 0)
            {
                _idaccount = (int)idacount;
                _loginuser = loginUser;
                _servicename = serviceName;
                _url = url;
                _loginaccount = loginAccount;
                _password = password;
            }
        }

        // Метод для обновления аккаунта
        public void UpdateAccount(int idAccount, string loginUser, string serviceName, string url, string loginAccount, string password)
        {
            OpenConnection();

            string query = @"UPDATE Accounts SET 
                             LoginUser = @LoginUser,
                             ServiceName = @ServiceName, 
                             URL = @URL, 
                             LoginAccount = @LoginAccount, 
                             Password = @Password 
                             WHERE IdAccount = @IdAccount;";
            int affectedRows;

            using (var command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@IdAccount", idAccount);
                command.Parameters.AddWithValue("@LoginUser", loginUser);
                command.Parameters.AddWithValue("@ServiceName", serviceName);
                command.Parameters.AddWithValue("@URL", url);
                command.Parameters.AddWithValue("@LoginAccount", loginAccount);
                command.Parameters.AddWithValue("@Password", password);

                affectedRows = command.ExecuteNonQuery();
            }

            CloseConnection();

            // Обновляем поля класса, если обновление прошло успешно
            if (affectedRows > 0)
            {
                _idaccount = idAccount;
                _loginuser = loginUser;
                _servicename = serviceName;
                _url = url;
                _loginaccount = loginAccount;
                _password = password;
            }
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

                        accountData["IdAccount"] = reader.GetInt32(reader.GetOrdinal("IdAccount")).ToString();
                        accountData["LoginUser"] = reader["LoginUser"].ToString();
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
            var AccountsList = new List<Dictionary<string, string>>();

            using (var command = new SQLiteCommand(query, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var AccountData = new Dictionary<string, string>
                        {
                            ["IdAccount"] = reader.GetInt32(reader.GetOrdinal("IdAccount")).ToString(),
                            ["LoginUser"] = reader["LoginUser"].ToString(),
                            ["ServiceName"] = reader["ServiceName"].ToString(),
                            ["URL"] = reader["URL"].ToString(),
                            ["LoginAccount"] = reader["LoginAccount"].ToString(),
                            ["Password"] = reader["Password"].ToString()
                        };
                        AccountsList.Add(AccountData);
                    }
                }
            }

            CloseConnection();
            return AccountsList;
        }
    }
}
