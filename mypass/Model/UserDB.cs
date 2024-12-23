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
        public UsersDB(string databasePath, string password)
        {
            _databasePath = databasePath;
            _passwordDB = password;
            _connectionString = $"Data Source={_databasePath};Version=3;Password={_passwordDB};";
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
        public void AddUser(string login, string firstname, string secondname,  string masterpasswordhash, string salt)
        {
            InitTransaction("Вызов метода 'AddUser'");
            OpenConnection();

            string query = "INSERT INTO User (LoginUser, FirstName, SecondName, MasterPasswordHash, Salt) VALUES (@LoginUser, @FirstName, @SecondName, @MasterPasswordHash, @Salt);";
            MessageError($"Создание запроса: {query}");
            int affectedRows;

            using (var command = _connection.CreateCommand())
            { 
                command.CommandText = query;
                MessageError($"Вызов команд для добавления");
                command.Parameters.AddWithValue("@LoginUser", login);
                command.Parameters.AddWithValue("@FirstName", firstname);
                command.Parameters.AddWithValue("@SecondName", secondname);
                command.Parameters.AddWithValue("@MasterPasswordHash", masterpasswordhash);
                command.Parameters.AddWithValue("@Salt", salt);

                affectedRows = command.ExecuteNonQuery();
                MessageError($"Значение переменной после завершения команд: {affectedRows}");
            }

            CloseConnection();

            // Обновляем поля класса, если обновление прошло успешно
            if (affectedRows > 0)
            {
                MessageError($"Применение полученных значений к полям класса");
                _loginuser = login;
                _firstname = firstname;
                _secondname = secondname;
                _masterpasswordhash = masterpasswordhash;
                _salt = salt;
            }

            CloseTransaction();
        }

        // Метод для обновления данных пользователя
        public void UpdateUser(string login, string newFirstname, string newSecondname, string newMasterPasswordHash, string newSalt)
        {
            InitTransaction("Вызов метода 'UpdateUser'");
            OpenConnection();

            string query = "UPDATE Users SET FirstName = @FirstName, SecondName = @SecondName, MasterPasswordHash = @MasterPasswordHash, Salt = @Salt " +
                "WHERE LoginUser = @LoginUser;";
            MessageError($"Создание запроса: {query}");
            int affectedRows;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                MessageError($"Вызов команд для обновления");
                command.Parameters.AddWithValue("@LoginUser", login);
                command.Parameters.AddWithValue("@FirstName", newFirstname);
                command.Parameters.AddWithValue("@SecondName", newSecondname);
                command.Parameters.AddWithValue("@MasterPasswordHash", newMasterPasswordHash);
                command.Parameters.AddWithValue("@Salt", newSalt);

                affectedRows = command.ExecuteNonQuery();
                MessageError($"Значение переменной после завершения команд: {affectedRows}");
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
                CloseTransaction();
            }
        }

        // Метод удаления базы данных (тк одна бд для одного пользователя)
        public void RemoveUser()
        {
            InitTransaction("Вызов метода 'RemoveUser'");
            bool result = false;

            try
            {
                File.Exists(_databasePath);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            MessageError($"Резульат попытки удаления БД: {result}");
            CloseTransaction();
        }

        // Метод для получения полной информации о пользователе
        public Dictionary<string, string> GetUserData(string login)
        {
            InitTransaction("Вызов метода 'GetUserData'");
            Dictionary<string, string> userDataDictionary = new Dictionary<string, string>();

            OpenConnection();

            string query = "SELECT FirstName, SecondName, MasterPasswordHash, Salt FROM Users WHERE LoginUser = @LoginUser;";
            MessageError($"Создание запроса: {query}");

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("LoginUser", login);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        MessageError($"Заполнение полей полученными данными");
                        _loginuser = reader["LoginUser"].ToString();
                        _firstname = reader["FirstName"].ToString();
                        _secondname = reader["SecondName"].ToString();
                        _masterpasswordhash = reader["MasterPasswordHash"].ToString();
                        _salt = reader["Salt"].ToString();

                        MessageError($"Добавление в словарь полученных данных");
                        userDataDictionary.Add("LoginUser", _loginuser);
                        userDataDictionary.Add("FirstName", _firstname);
                        userDataDictionary.Add("SecondName", _secondname);
                        userDataDictionary.Add("MasterPasswordHash", _masterpasswordhash);
                        userDataDictionary.Add("Salt", _salt);
                    }
                    else
                    {
                        MessageError($"Пользователь не найден");
                    }
                }
            }
            
            CloseConnection();
            CloseTransaction();
            return userDataDictionary;
        }

        public void LoadDataFromUserDB()
        {
            InitTransaction("Начало загрузки данных из UserDB");
            OpenConnection();

            string query = "SELECT LoginUser, FirstName, SecondName, MasterPasswordHash, Salt FROM User;";
            MessageError($"Создание запроса: {query}");

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;

                using (var reader = command.ExecuteReader())
                {
                    MessageError("Применеие загруженных значений к полям");
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
            CloseTransaction();
        }
    }
}
