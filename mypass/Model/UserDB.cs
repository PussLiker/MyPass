using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Microsoft.Data.Sqlite;

// Зачем нужен: типо таблица с бд
// Наследование: лень писать
// Методы: лень писать
// Пример использования: лень писать

namespace mypass.Model
{
    
    // Класс для таблицы Users
    public class UsersDB : DataBase
    {
        public UsersDB(string databasePath)
        {
            _databasePath = databasePath;
            _connectionString = $"Data Source={_databasePath};Version=3;";
            _connection = new SQLiteConnection(_connectionString);
        }

        // Поля класса
        private string _loginuser;
        public string LoginUser
        {
            get => _loginuser;
            set => _loginuser = value;
        }
        private string _firstname;
        public string FirstName
        {
            get => _firstname;
            set => _firstname = value;
        }
        private string _secondname;
        public string SecondName
        {
            get => _secondname;
            set => _secondname = value;
        }
        private string _masterpasswordhash;
        public string MasterPasswordHash
        {
            get => _masterpasswordhash;
            set => _masterpasswordhash = value;
        }
        private string _salt;
        public string Salt
        {
            get => _salt;
            set => _salt = value;
        }

        // Метод для добавления нового пользователя
        public void AddUser(string login, string firstname, string secondname, string masterpasswordhash, string salt)
        {
            OpenConnection();

            string query = "INSERT INTO User (LoginUser, FirstName, SecondName, MasterPasswordHash, Salt) VALUES (@LoginUser, @FirstName, @SecondName, @MasterPasswordHash, @Salt);";
            int affectedRows;

            using (var command = _connection.CreateCommand())
            { 
                command.CommandText = query;
                command.Parameters.AddWithValue("@LoginUser", login);
                command.Parameters.AddWithValue("@FirstName", firstname);
                command.Parameters.AddWithValue("@SecondName", secondname);
                command.Parameters.AddWithValue("@MasterPasswordHash", masterpasswordhash);
                command.Parameters.AddWithValue("@Salt", salt);

                affectedRows = command.ExecuteNonQuery();
            }

            CloseConnection();

            // Обновляем поля класса, если обновление прошло успешно
            if (affectedRows > 0)
            {
                _loginuser = login;
                _firstname = firstname;
                _secondname = secondname;
                _salt = salt;
            }
        }

        // Метод для обновления данных пользователя
        public void UpdateUser(string login, string newFirstname, string newSecondname, string newMasterPasswordHash, string newSalt)
        {
            OpenConnection();

            string query = "UPDATE User SET FirstName = @FirstName, SecondName = @SecondName, MasterPasswordHash = @MasterPasswordHash, Salt = @Salt " +
                "WHERE LoginUser = @LoginUser;";
            int affectedRows;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@LoginUser", login);
                command.Parameters.AddWithValue("@FirstName", newFirstname);
                command.Parameters.AddWithValue("@SecondName", newSecondname);
                command.Parameters.AddWithValue("@MasterPasswordHash", newMasterPasswordHash);
                command.Parameters.AddWithValue("@Salt", newSalt);

                affectedRows = command.ExecuteNonQuery();
            }

            CloseConnection();

            // Обновляем поля класса, если обновление прошло успешно
            if (affectedRows > 0)
            {
                _loginuser = login;
                _firstname = newFirstname;
                _secondname = newSecondname;
                _masterpasswordhash = newMasterPasswordHash;
                _salt = newSalt;
            }
        }

        // Метод удаления базы данных (тк одна бд для одного пользователя)
        public void RemoveUser()
        {
            try
            {
                File.Exists(_databasePath);
            }
            catch (Exception)
            {
                //
            }
        }

        // Метод для получения полной информации о пользователе
        public Dictionary<string, string> GetUserData(string login)
        {
            Dictionary<string, string> userDataDictionary = new Dictionary<string, string>();

            OpenConnection();

            string query = "SELECT FirstName, SecondName, MasterPasswordHash, Salt FROM User WHERE LoginUser = @LoginUser;";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("LoginUser", login);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        _loginuser = reader["LoginUser"].ToString();
                        _firstname = reader["FirstName"].ToString();
                        _secondname = reader["SecondName"].ToString();
                        _masterpasswordhash = reader["MasterPasswordHash"].ToString();
                        _salt = reader["Salt"].ToString();

                        userDataDictionary.Add("LoginUser", _loginuser);
                        userDataDictionary.Add("FirstName", _firstname);
                        userDataDictionary.Add("SecondName", _secondname);
                        userDataDictionary.Add("MasterPasswordHash", _masterpasswordhash);
                        userDataDictionary.Add("Salt", _salt);
                    }
                    else
                    {
                        //
                    }
                }
            }
            
            CloseConnection();
            return userDataDictionary;
        }

        public void LoadDataFromUserDB()
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
                        _loginuser = reader["LoginUser"].ToString();
                        _firstname = reader["FirstName"].ToString();
                        _secondname = reader["SecondName"].ToString();
                        _masterpasswordhash = reader["MasterPasswordHash"].ToString();
                        _salt = reader["Salt"].ToString();
                    }
                }
            }
            CloseConnection();
        }
    }
}
