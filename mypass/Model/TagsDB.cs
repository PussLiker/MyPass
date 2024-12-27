using System.Collections.Generic;

using System.Data.SQLite;


namespace mypass.Model
{
    // Класс для работы с таблицей Tags
    public class TagsDB : DataBase
    {
        public TagsDB(string databasePath)
        {
            _databasePath = databasePath;
            _connectionString = $"Data Source={_databasePath};Version=3;";
            _connection = new SQLiteConnection(_connectionString);
        }

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
        public void AddTag(string nameTag)
        {
            OpenConnection();

            int affectedRows;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Tags (NameTag) VALUES (@NameTag);";
                command.Parameters.AddWithValue("@NameTag", nameTag);

                affectedRows = command.ExecuteNonQuery();
            }

            long idtag = _connection.LastInsertRowId;

            CloseConnection();

            if (affectedRows > 0)
            {
                _idtag = (int)idtag;
                _nametag = nameTag;
            }
        }

        // Метод для обновления тега
        public void UpdateTag(int idTag, string newnameTag)
        {
            OpenConnection();

            int affectedRows;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE Tags SET NameTag = @NameTag WHERE IdTag = @IdTag;";
                command.Parameters.AddWithValue("@NameTag", newnameTag);
                command.Parameters.AddWithValue("@IdTag", idTag);

                affectedRows = command.ExecuteNonQuery();
            }

            CloseConnection();

            if (affectedRows > 0)
            {
                _idtag = idTag;
                _nametag = newnameTag;
            }
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
        public Dictionary<string, string> GetTag(int idTag)
        {
            OpenConnection();
            var result = new Dictionary<string, string>();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Tags WHERE IdTag = @IdTag;";
                command.Parameters.AddWithValue("@IdTag", idTag);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result["IdTag"] = reader.GetInt32(reader.GetOrdinal("IdAcIdTagtion")).ToString();
                        result["NameTag"] = reader["NameTag"].ToString();
                    }
                }
            }

            CloseConnection();
            return result;
        }

        // Метод для загрузки данных из таблицы Tags
        public List<Dictionary<string, string>> GetAllTags()
        {
            OpenConnection();
            var TagsList = new List<Dictionary<string, string>>();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT IdTag, NameTag FROM Tags;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var TagsData = new Dictionary<string, string>
                        {
                            ["IdTag"] = reader.GetInt32(reader.GetOrdinal("IdTag")).ToString(),
                            ["NameTag"] = reader["NameTag"].ToString(),
                        };
                        TagsList.Add(TagsData);
                    }
                }
            }

            CloseConnection();
            return TagsList;
        }
    }
}
