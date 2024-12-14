using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;
using System.Data.SQLite;
using System.Data.Common;
using System.Xml.Linq;

namespace mypass.Model
{
    internal class UsersDB : DataBase
    {
        private int _id;
        public int ID
        {
            get => _id; // Проверить нужно ли return
            set => _id = value;
        }
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
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

        // Конструктор для инициализации ID пользователя и строки подключения
        public UsersDB(string databasePath, string password, int id, string name, string masterpasswordhash, string salt)
            : base(databasePath, password)
        {
            _id = id;
            _name = name;
            _masterpasswordhash = masterpasswordhash;
            _salt = salt;
        }

        // Метод для добавления нового пользователя
        public void AddUser(string name, string masterpasswordhash, string salt)
        {
            OpenConnection();

            string query = "INSERT INTO 'Users' (Username, MasterPasswordHash, Salt) VALUES (@Username, @MasterPasswordHash, @Salt);";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@Username", name);
                command.Parameters.AddWithValue("@MasterPasswordHash", masterpasswordhash);
                command.Parameters.AddWithValue("@Salt", salt);
                command.ExecuteNonQuery();
            }

            CloseConnection();
        }

        // Метод удаления нового пользователя
        public void RemoveUser(string name)
        {
            OpenConnection();

            string query = "DELETE FROM Users WHERE Username = @Username;";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@Username", name);
                command.ExecuteNonQuery();
            }

            CloseConnection();
        }

        // Метод для получения полной информации о "Users"
        public Dictionary<string, string> GetUserData(int iduser)
        {
            Dictionary<string, string> userDataDictionary = new Dictionary<string, string>();

            OpenConnection();

            string query = "SELECT IdUser, Username, MasterPasswordHash, Salt FROM Users WHERE IdUser = @iduser;";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@iduser", iduser);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        userDataDictionary.Add("IdUser", reader["IdUser"].ToString());
                        userDataDictionary.Add("Username", reader["Username"].ToString());
                        userDataDictionary.Add("MasterPasswordHash", reader["MasterPasswordHash"].ToString());
                        userDataDictionary.Add("Salt", reader["Salt"].ToString());
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
