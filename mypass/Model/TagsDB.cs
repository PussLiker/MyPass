using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

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

            string query = "INSERT INTO Tags (NameTag) VALUES (@NameTag);";
            int affectedRows;

            using (var command = new SqliteCommand(query, _connection))
            {
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

            string query = "UPDATE Tags SET NameTag = @NameTag WHERE IdTag = @IdTag;";
            int affectedRows;

            using (var command = new SqliteCommand(query, _connection))
            {
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

            string query = "DELETE FROM Tags WHERE IdTag = @IdTag;";
            int affectedRows;

            using (var command = new SqliteCommand(query, _connection))
            {
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

            using (var command = new SqliteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@IdTag", idTag);

                using (var reader = command.ExecuteReader())
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

        // Метод для загрузки данных из таблицы Tags
        public void LoadDataFromTagsDB()
        {
            OpenConnection();

            string query = "SELECT IdTag, NameTag FROM Tags;";

            using (var command = new SqliteCommand(query, _connection))
            {
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
