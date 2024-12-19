using System.Collections.Generic;

namespace mypass.Model
{
    // Класс для работы с таблицей Tags
    public class TagsDB : DataBase
    {
        private int _id;
        public int ID
        {
            get => _id;
            set => _id = value;
        }
        private string _nametag;
        public string NameTag
        {
            get => _nametag;
            set => _nametag = value;
        }
        public TagsDB(string databasePath, string password) : base(databasePath, password) { }

        // Метод для добавления тега
        public void AddTag(string nameTag)
        {
            OpenConnection();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Tags (NameTag) VALUES (@NameTag);";
                command.Parameters.AddWithValue("@NameTag", nameTag);
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        // Метод для обновления тега
        public void UpdateTag(int idTag, string newNameTag)
        {
            OpenConnection();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE Tags SET NameTag = @NameTag WHERE IdTag = @IdTag;";
                command.Parameters.AddWithValue("@NameTag", newNameTag);
                command.Parameters.AddWithValue("@IdTag", idTag);
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        // Метод для удаления тега
        public void DeleteTag(int idTag)
        {
            OpenConnection();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Tags WHERE IdTag = @IdTag;";
                command.Parameters.AddWithValue("@IdTag", idTag);
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        // Метод для получения информации о теге
        public Dictionary<string, string> GetTagById(int idTag)
        {
            OpenConnection();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Tags WHERE IdTag = @IdTag;";
                command.Parameters.AddWithValue("@IdTag", idTag);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var tagData = new Dictionary<string, string>
                        {
                            { "IdTag", reader["IdTag"].ToString() },
                            { "NameTag", reader["NameTag"].ToString() }
                        };
                        CloseConnection();
                        return tagData;
                    }
                }
            }
            CloseConnection();
            return null;
        }
    }
}