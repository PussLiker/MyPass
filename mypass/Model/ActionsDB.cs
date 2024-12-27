using System;
using System.Collections.Generic;

using System.Data.SQLite;


namespace mypass.Model
{
    public class ActionsDB : DataBase
    {
        public ActionsDB(string databasePath)
        {
            _databasePath = databasePath;
            _connectionString = $"Data Source={_databasePath};Version=3;";
            _connection = new SQLiteConnection(_connectionString);
        }

        private int _idaction;
        public int IdAction
        {
            get => _idaction;
            set => _idaction = value;
        }
        private int _idaccount;
        public int IdAccount
        {
            get => _idaccount;
            set => _idaccount = value;
        }
        private int _idevent;
        public int IdEvent
        {
            get => _idevent;
            set => _idevent = value;
        }
        private DateTime _timeevent;
        public DateTime TimeEvent
        {
            get => _timeevent;
            set => _timeevent = value;
        }

        public void AddAction(int idAccount, int idEvent, DateTime timeEvent)
        {
            OpenConnection();
            string query = "INSERT INTO Actions (IdAccount, IdEvent, TimeEvent) VALUES (@IdAccount, @IdEvent, @TimeEvent);";

            int affectedRows;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdAccount", idAccount);
                command.Parameters.AddWithValue("@IdEvent", idEvent);
                command.Parameters.AddWithValue("@TimeEvent", timeEvent);

                affectedRows = command.ExecuteNonQuery();
            }

            long idaction = _connection.LastInsertRowId;

            CloseConnection();

            if (affectedRows > 0)
            {
                _idaction = (int)idaction;
                _idaccount = idAccount;
                _idevent = idEvent;
                _timeevent = timeEvent;
            }
        }

        // Метод для обновления записи
        public void UpdateAction(int idAction, int newidAccount, int newidEvent, DateTime newtimeEvent)
        {
            OpenConnection();
            string query = @"UPDATE Actions 
                             SET IdAccount = @IdAccount, IdEvent = @IdEvent, TimeEvent = @TimeEvent 
                             WHERE IdAction = @IdAction;";

            int affectedRows;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                
                command.Parameters.AddWithValue("@IdAccount", newidAccount);
                command.Parameters.AddWithValue("@IdEvent", newidEvent);
                command.Parameters.AddWithValue("@TimeEvent", newtimeEvent);
                command.Parameters.AddWithValue("@IdAction", idAction);

                affectedRows = command.ExecuteNonQuery();
            }

            CloseConnection();

            if (affectedRows > 0)
            {
                _idaction = idAction;
                _idaccount = newidAccount;
                _idevent = newidEvent;
                _timeevent = newtimeEvent;
            }
        }

        // Метод для удаления записи
        public void DeleteAction(int idAction)
        {
            OpenConnection();
            string query = "DELETE FROM Actions WHERE IdAction = @IdAction;";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdAction", idAction);

                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        // Метод для получения записи в виде Dictionary
        public Dictionary<string, string> GetActionById(int idAction)
        {
            OpenConnection();
            string query = "SELECT * FROM Actions WHERE IdAction = @IdAction;";
            var result = new Dictionary<string, string>();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdAction", idAction);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result["IdAction"] = reader.GetInt32(reader.GetOrdinal("IdAction")).ToString();
                        result["IdAccount"] = reader.GetInt32(reader.GetOrdinal("IdAccount")).ToString();
                        result["IdEvent"] = reader.GetInt32(reader.GetOrdinal("IdEvent")).ToString();

                        result["TimeEvent"] = reader.GetDateTime(4).ToString("o"); // ПРОТЕСТИТЕ ПОЖАЛУЙСТА ЕСЛИ НА СРАБОТАЕТ 3 ПОСТАВЬТЕ 4
                    }
                }
            }
            CloseConnection();
            return result;
        }

        public List<Dictionary<string, string>> GetAllActions()
        {
            OpenConnection();

            var ActionsList = new List<Dictionary<string, string>>();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT IdAction, IdAccount, IdEvent, TimeEvent FROM Actions;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ActionsData = new Dictionary<string, string>
                        {
                            ["IdAction"] = reader.GetInt32(reader.GetOrdinal("IdAction")).ToString(),
                            ["IdAccount"] = reader.GetInt32(reader.GetOrdinal("IdAccount")).ToString(),
                            ["IdEvent"] = reader.GetInt32(reader.GetOrdinal("IdEvent")).ToString(),

                            ["TimeEvent"] = reader.GetDateTime(4).ToString("o") // ТУТ ТАКАЯ ЖЕ ЗАЛУПА НАДО ПОМЕНЯТЬ НА 4 ЕСЛИ 3 НЕ РАБОТАЕТ
                        };
                        ActionsList.Add(ActionsData);
                    }
                }
            }

            CloseConnection();
            return ActionsList;
        }
    }
}
