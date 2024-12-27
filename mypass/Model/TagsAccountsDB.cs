using System;
using System.Collections.Generic;

using System.Data.SQLite;

namespace mypass.Model
{
    // Класс для работы с таблицей TagsAccounts
    public class TagsAccountsDB : DataBase
    {
        public TagsAccountsDB(string databasePath)
        {
            _databasePath = databasePath;
            _connectionString = $"Data Source={_databasePath};Version=3;";
            _connection = new SQLiteConnection(_connectionString);
        }

        private int _idtagsaccounts;
        public int IdTagsAccounts
        {
            get => _idtagsaccounts;
            set => _idtagsaccounts = value;
        }
        private int _idaccount;
        public int IdAccount
        {
            get => _idaccount;
            set => _idaccount = value;
        }
        private int _idtag;
        public int IdTag
        {
            get => _idtag;
            set => _idtag = value;
        }
        private DateTime _timetagging;
        public DateTime TimeTagging
        {
            get => (DateTime)_timetagging;
            set => _timetagging = value;
        }

        // Метод для добавления связи между тегом и аккаунтом
        public void AddTagAccount(int idAccount, int idTag, DateTime timeTagging)
        {
            OpenConnection();

            int affectedRows;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO TagsAccounts (IdAccount, IdTag, TimeTagging) VALUES (@IdAccount, @IdTag, @TimeTagging);";
                command.Parameters.AddWithValue("@IdAccount", idAccount);
                command.Parameters.AddWithValue("@IdTag", idTag);
                command.Parameters.AddWithValue("@TimeTagging", timeTagging);
                affectedRows = command.ExecuteNonQuery();
            }

            long idtagsaccounts = _connection.LastInsertRowId;

            CloseConnection();

            if (affectedRows > 0)
            {
                _idtagsaccounts = (int)idtagsaccounts;
                _idaccount = idAccount;
                _idtag = idTag;
                _timetagging = timeTagging;
            }
        }

        // Метод для обновления связи
        public void UpdateTagAccount(int idTagsAccounts, int newidAccount, int newidTag, DateTime newTimeTagging)
        {
            OpenConnection();

            int affectedRows;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE TagsAccounts SET IdAccount = @IdAccount, IdTag = @IdTag, TimeTagging = @TimeTagging WHERE IdTagsAccounts = @IdTagsAccounts;";

                command.Parameters.AddWithValue("@IdAccount", newidAccount);
                command.Parameters.AddWithValue("@IdTag", newidTag);
                command.Parameters.AddWithValue("@TimeTagging", newTimeTagging);
                command.Parameters.AddWithValue("@IdTagsAccounts", idTagsAccounts);
                affectedRows = command.ExecuteNonQuery();
            }
            CloseConnection();

            if (affectedRows > 0)
            {
                _idtagsaccounts = idTagsAccounts;
                _idaccount = newidAccount;
                _idtag = newidTag;
                _timetagging = newTimeTagging;
            }
        }

        // Метод для удаления связи
        public void DeleteTagAccount(int idTagsAccounts)
        {
            OpenConnection();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM TagsAccounts WHERE IdTagsAccounts = @IdTagsAccounts;";
                command.Parameters.AddWithValue("@IdTagsAccounts", idTagsAccounts);
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        // Метод для получения информации о связи
        public Dictionary<string, string> GetTagAccountById(int idTagsAccounts)
        {
            OpenConnection();
            var result = new Dictionary<string, string>();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM TagsAccounts WHERE IdTagsAccounts = @IdTagsAccounts;";
                command.Parameters.AddWithValue("@IdTagsAccounts", idTagsAccounts);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                        result["IdTagsAccounts"] = reader.GetInt32(reader.GetOrdinal("IdTagsAccounts")).ToString();
                        result["IdAccount"] = reader.GetInt32(reader.GetOrdinal("IdAccount")).ToString();
                        result["IdTag"] = reader.GetInt32(reader.GetOrdinal("IdTag")).ToString();

                        result["TimeTagging"] = reader.GetDateTime(4).ToString("o"); // ПРОТЕСТИТЕ ПОЖАЛУЙСТА ЕСЛИ НА СРАБОТАЕТ 3 ПОСТАВЬТЕ 4

                        
                        CloseConnection();
                        return result;
                    }
                }
            }
            CloseConnection();
            return null;
        }

        public List<Dictionary<string, string>> GetAllTags()
        {
            OpenConnection();

            var TagsAccountsList = new List<Dictionary<string, string>>();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT IdTagsAccounts, IdAccount, IdTag, TimeTagging FROM TagsAccounts;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var TagsAccountsData = new Dictionary<string, string>
                        {
                            ["IdTagsAccounts"] = reader.GetInt32(reader.GetOrdinal("IdTagsAccounts")).ToString(),
                            ["IdAccount"] = reader.GetInt32(reader.GetOrdinal("IdAccount")).ToString(),
                            ["IdTag"] = reader.GetInt32(reader.GetOrdinal("IdTag")).ToString(),

                            ["TimeTagging"] = reader.GetDateTime(4).ToString("o") // ТУТ ТАКАЯ ЖЕ ЗАЛУПА НАДО ПОМЕНЯТЬ НА 4 ЕСЛИ 3 НЕ РАБОТАЕТ
                        };
                        TagsAccountsList.Add(TagsAccountsData);
                    }
                }
            }

            CloseConnection();
            return TagsAccountsList;
        }
    }
}
