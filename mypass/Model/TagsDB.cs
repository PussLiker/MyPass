using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Security.Cryptography;

// Зачем нужен: типо таблица с бд
// Наследование: лень писать
// Методы: лень писать
// Пример использования: лень писать

namespace mypass.Model
{
    // Класс для работы с таблицей Tags
    public class TagsDB : DataBase
    {
        private int _idtag;
        public int IdTag
        {
            get => _idtag;
            set => _idtag = value;
        }
        private string _nametag;
        public string NameTag
        {
            get => _nametag;
            set => _nametag = value;
        }

        // Метод для добавления тега
        public void AddTag(int idTag, string nameTag)
        {
            OpenConnection();

            int affectedRows;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Tags (NameTag) VALUES (@NameTag);";
                command.Parameters.AddWithValue("@NameTag", nameTag);

                affectedRows = command.ExecuteNonQuery();
            }

            CloseConnection();

            // Обновляем поля класса, если обновление прошло успешно
            if (affectedRows > 0)
            {
                _idtag = idTag;
                _nametag = nameTag;
            }
        }

        // Метод для обновления тега
        public void UpdateTag(int idTag, string newNameTag)
        {
            OpenConnection();

            int affectedRows;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE Tags SET NameTag = @NameTag WHERE IdTag = @IdTag;";
                command.Parameters.AddWithValue("@NameTag", newNameTag);
                command.Parameters.AddWithValue("@IdTag", idTag);
                affectedRows = command.ExecuteNonQuery();
            }
            CloseConnection();

            // Обновляем поля класса, если обновление прошло успешно
            if (affectedRows > 0)
            {
                _idtag = idTag;
                _nametag = newNameTag;
            }
        }

        // Метод для удаления тега
        public void DeleteTag(int idTag)
        {
            OpenConnection();

            int affectedRows;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Tags WHERE IdTag = @IdTag;";
                command.Parameters.AddWithValue("@IdTag", idTag);
                affectedRows = command.ExecuteNonQuery();
            }
            CloseConnection();

            // Обновляем поля класса, если обновление прошло успешно
            if (affectedRows > 0)
            {
                _nametag = "";
            }
        }

        // Метод для получения информации о теге
        public Dictionary<string, string> GetTagById(int idTag)
        {
            OpenConnection();
            string query = "SELECT * FROM Tags WHERE IdTag = @IdTag;";
            var result = new Dictionary<string, string>();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdTag", idTag);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        _idtag = Convert.ToInt32(reader["IdTag"]);
                        _nametag = reader["NameTag"].ToString();

                        result["IdTag"] = reader["IdTag"].ToString();
                        result["NameTag"] = reader["NameTag"].ToString();
                    }
                }
            }
            CloseConnection();
            return result;
        }

        public void LoadDataFromTagsDB()
        {
            OpenConnection();

            string query = "SELECT IdTag, NameTag FROM Tags;";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _idtag = Convert.ToInt32(reader["IdTag"]);
                        _nametag = reader["NameTag"].ToString();
                    }
                }
            }
            CloseConnection();
        }
    }
}