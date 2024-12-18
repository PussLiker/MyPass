using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace mypass.Model
{
    // Класс для работы с таблицей TagsAccounts
    public class TagsAccountsDB : DataBase
    {
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
        public TagsAccountsDB(string databasePath, string password) : base(databasePath, password) { }

        // Метод для добавления связи между тегом и аккаунтом
        public void AddTagAccount(int idAccount, int idTag, DateTime timeTagging)
        {
            OpenConnection();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO TagsAccounts (IdAccount, IdTag, TimeTagging) VALUES (@IdAccount, @IdTag, @TimeTagging);";
                command.Parameters.AddWithValue("@IdAccount", idAccount);
                command.Parameters.AddWithValue("@IdTag", idTag);
                command.Parameters.AddWithValue("@TimeTagging", timeTagging);
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        // Метод для обновления связи
        public void UpdateTagAccount(int idTagsAccounts, int idAccount, int idTag, DateTime newTimeTagging)
        {
            OpenConnection();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE TagsAccounts SET IdAccount = @IdAccount, IdTag = @IdTag, TimeTagging = @TimeTagging WHERE IdTagsAccounts = @IdTagsAccounts;";
                command.Parameters.AddWithValue("@IdTagsAccounts", idTagsAccounts);
                command.Parameters.AddWithValue("@IdAccount", idAccount);
                command.Parameters.AddWithValue("@IdTag", idTag);
                command.Parameters.AddWithValue("@TimeTagging", newTimeTagging);
                command.ExecuteNonQuery();
            }
            CloseConnection();
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
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM TagsAccounts WHERE IdTagsAccounts = @IdTagsAccounts;";
                command.Parameters.AddWithValue("@IdTagsAccounts", idTagsAccounts);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var tagAccountData = new Dictionary<string, string>
                        {
                            { "IdTagsAccounts", reader["IdTagsAccounts"].ToString() },
                            { "IdAccount", reader["IdAccount"].ToString() },
                            { "IdTag", reader["IdTag"].ToString() },
                            { "TimeTagging", reader["TimeTagging"].ToString() }
                        };
                        CloseConnection();
                        return tagAccountData;
                    }
                }
            }
            CloseConnection();
            return null;
        }
    }
}
