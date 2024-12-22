using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;

// Зачем нужен: Этот класс нужен для упралвения созданием и установкой пароля для базы данных
// Наследование: идёт в наследование путь к созданной бд (_databasePath) и пароль пользователя (_passwordDB)
// Методы: 'CreateDataBase' - для создания БД, а также папки 'DataBase', если ранее не была создана и EncryptDataBase - для установки пользовательского пароля для бд

// Пример использования: DataBaseManager.CreateDataBase('Nikita');
//                       DataBaseManager.EncryptDataBase('230rt0450Tkkgji4'); - также можно вызывать много раз, тк с 59 по 62 строчку идёт проверка на уже имеющийся парол


namespace mypass.Model
{
    // Класс для таблицы Users
    public class UsersDB : DataBase
    {
        // Поля класса
        private int _id;
        public int ID
        {
            get => _id;
            set => _id = value;
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
        private string _username;
        public string UserName
        {
            get => _username;
            set => _username = value;
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
        public bool AddUser(int idUser, string firstname, string secondname, string username, string masterpasswordhash, string salt)
        {
            InitTransaction("Вызов метода 'AddUser'");
            OpenConnection();

            string query = "INSERT INTO Users (IdUser, FirstName, SecondName, Username, MasterPasswordHash, Salt) VALUES (@IdUsers, @FirstName, @SecondName @Username, @MasterPasswordHash, @Salt);";
            MessageError($"Создание запроса: {query}");
            int affectedRows;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                MessageError($"Вызов команд для добавления");
                command.Parameters.AddWithValue("@IdUser", idUser);
                command.Parameters.AddWithValue("@FirstName", firstname);
                command.Parameters.AddWithValue("@SecondName", secondname);
                command.Parameters.AddWithValue("@Username", username);
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
                _id = idUser;
                _firstname = firstname;
                _secondname = secondname;
                _username = username;
                _masterpasswordhash = masterpasswordhash;
                _salt = salt;
            }

            CloseTransaction();
            return affectedRows > 0;
        }

        // Метод для обновления данных пользователя
        public bool UpdateUser(int idUser, string newFirstname, string newSecondname, string newName, string newMasterPasswordHash, string newSalt)
        {
            InitTransaction("Вызов метода 'UpdateUser'");
            OpenConnection();

            string query = "UPDATE Users SET FirstName = @FirstName, SecondName = @SecondName, Username = @Username, MasterPasswordHash = @MasterPasswordHash, Salt = @Salt WHERE IdUser = @IdUser;";
            MessageError($"Создание запроса: {query}");
            int affectedRows;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                MessageError($"Вызов команд для обновления");
                command.Parameters.AddWithValue("@IdUser", idUser);
                command.Parameters.AddWithValue("@FirstName", newFirstname);
                command.Parameters.AddWithValue("@SecondName", newSecondname);
                command.Parameters.AddWithValue("@Username", newName);
                command.Parameters.AddWithValue("@MasterPasswordHash", newMasterPasswordHash);
                command.Parameters.AddWithValue("@Salt", newSalt);

                affectedRows = command.ExecuteNonQuery();
                MessageError($"Значение переменной после завершения команд: {affectedRows}");
            }

            CloseConnection();

            // Обновляем поля класса, если обновление прошло успешно
            if (affectedRows > 0)
            {
                _id = idUser;
                _firstname = newFirstname;
                _secondname = newSecondname;
                _username = newName;
                _masterpasswordhash = newMasterPasswordHash;
                _salt = newSalt;
            }

            CloseTransaction();
            return affectedRows > 0;
        }

        // Метод удаления базы данных (тк одна бд для одного пользователя)
        public bool RemoveUser()
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
            return result;
        }

        // Метод для получения полной информации о пользователе
        public Dictionary<string, string> GetUserData(int idUser)
        {
            InitTransaction("Вызов метода 'GetUserData'");
            Dictionary<string, string> userDataDictionary = new Dictionary<string, string>();

            OpenConnection();

            string query = "SELECT IdUser, FirstName, SecondName, Username, MasterPasswordHash, Salt FROM Users WHERE IdUser = @IdUser;";
            MessageError($"Создание запроса: {query}");

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdUser", idUser);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        MessageError($"Заполнение полей полученными данными");
                        _id = Convert.ToInt32(reader["IdUser"]);
                        _firstname = reader["FirstName"].ToString();
                        _secondname = reader["SecondName"].ToString();
                        _username = reader["Username"].ToString();
                        _masterpasswordhash = reader["MasterPasswordHash"].ToString();
                        _salt = reader["Salt"].ToString();

                        MessageError($"Добавление в словарь полученных данных");
                        userDataDictionary.Add("IdUser", _id.ToString());
                        userDataDictionary.Add("FirstName", _firstname);
                        userDataDictionary.Add("SecondName", _secondname);
                        userDataDictionary.Add("Username", _username);
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
    }
}
