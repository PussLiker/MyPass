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

        private string _username;
        public string UserName
        {
            get => _username;
            set => _username = value;
        }

        private string _masterPasswordHash;
        public string MasterPasswordHash
        {
            get => _masterPasswordHash;
            set => _masterPasswordHash = value;
        }

        private string _salt;
        public string Salt
        {
            get => _salt;
            set => _salt = value;
        }

        public UsersDB(string databasePath, string password) : base() { }

        // Метод для добавления нового пользователя
        public bool AddUser(int idUser, string name, string masterPasswordHash, string salt)
        {
            OpenConnection();

            string query = "INSERT INTO Users (IdUser, Username, MasterPasswordHash, Salt) VALUES (@IdUsers, @Username, @MasterPasswordHash, @Salt);";
            int affectedRows;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdUser", idUser);
                command.Parameters.AddWithValue("@Username", name);
                command.Parameters.AddWithValue("@MasterPasswordHash", masterPasswordHash);
                command.Parameters.AddWithValue("@Salt", salt);

                affectedRows = command.ExecuteNonQuery();
            }

            CloseConnection();

            // Обновляем поля класса, если обновление прошло успешно
            if (affectedRows > 0)
            {
                _id = idUser;
                _username = name;
                _masterPasswordHash = masterPasswordHash;
                _salt = salt;
            }

            return affectedRows > 0;
        }

        // Метод для обновления данных пользователя
        public bool UpdateUser(int idUser, string newName, string newPasswordHash, string newSalt)
        {
            OpenConnection();

            string query = "UPDATE Users SET Username = @Username, MasterPasswordHash = @MasterPasswordHash, Salt = @Salt WHERE IdUser = @IdUser;";
            int affectedRows;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdUser", idUser);
                command.Parameters.AddWithValue("@Username", newName);
                command.Parameters.AddWithValue("@MasterPasswordHash", newPasswordHash);
                command.Parameters.AddWithValue("@Salt", newSalt);

                affectedRows = command.ExecuteNonQuery();
            }

            CloseConnection();

            // Обновляем поля класса, если обновление прошло успешно
            if (affectedRows > 0)
            {
                _id = idUser;
                _username = newName;
                _masterPasswordHash = newPasswordHash;
                _salt = newSalt;
            }

            return affectedRows > 0;
        }

        // Метод удаления пользователя и базы данных, если он последний
        public bool RemoveUser(string name)
        {
            OpenConnection();

            string checkQuery = "SELECT COUNT(*) FROM Users;";
            int userCount;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = checkQuery;
                userCount = Convert.ToInt32(command.ExecuteScalar());
            }

            string deleteQuery = "DELETE FROM Users WHERE Username = @Username;";
            int affectedRows = 0;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = deleteQuery;
                command.Parameters.AddWithValue("@Username", name);
                affectedRows = command.ExecuteNonQuery();
            }

            CloseConnection();

            // Если это был последний пользователь, удаляем всю базу данных
            if (userCount == 1 && affectedRows > 0)
            {
                File.Delete(_databasePath);
                Console.WriteLine("Последний пользователь удалён. База данных также удалена.");
            }

            return affectedRows > 0;
        }

        // Метод для получения полной информации о пользователе
        public Dictionary<string, string> GetUserData(int idUser)
        {
            Dictionary<string, string> userDataDictionary = new Dictionary<string, string>();

            OpenConnection();

            string query = "SELECT IdUser, Username, MasterPasswordHash, Salt FROM Users WHERE IdUser = @IdUser;";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdUser", idUser);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        _id = Convert.ToInt32(reader["IdUser"]);
                        _username = reader["Username"].ToString();
                        _masterPasswordHash = reader["MasterPasswordHash"].ToString();
                        _salt = reader["Salt"].ToString();

                        userDataDictionary.Add("IdUser", _id.ToString());
                        userDataDictionary.Add("Username", _username);
                        userDataDictionary.Add("MasterPasswordHash", _masterPasswordHash);
                        userDataDictionary.Add("Salt", _salt);
                    }
                    else
                    {
                        Console.WriteLine("Пользователь не найден.");
                    }
                }
            }

            CloseConnection();
            return userDataDictionary;
        }
    }
}
