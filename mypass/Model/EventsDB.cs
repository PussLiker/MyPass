using System.Collections.Generic;
using System.Data.SQLite;

namespace mypass.Model
{
    public class EventsDB : DataBase
    {
        public EventsDB(string databasePath)
        {
            _databasePath = databasePath;
            _connectionString = $"Data Source={_databasePath};Version=3;";
            _connection = new SQLiteConnection(_connectionString);
        }

        private int _idevent;
        public int IdEvent
        {
            get => _idevent;
            set => _idevent = value;
        }
        private int _idtypeevent;
        public int IdTypeEvent
        {
            get => _idtypeevent;
            set => _idtypeevent = value;
        }
        private string _nameevent;
        public string NameEvent
        {
            get => _nameevent;
            set => _nameevent = value;
        }

        public void AddEvent(int idTypeEvent, string nameEvent)
        {
            OpenConnection();
            string query = "INSERT INTO Events (IdTypeEvent, NameEvent) VALUES (@IdTypeEvent, @NameEvent);";

            int affectedRows;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdTypeEvent", idTypeEvent);
                command.Parameters.AddWithValue("@NameEvent", nameEvent);

                affectedRows = command.ExecuteNonQuery();
            }

            long idevent = _connection.LastInsertRowId;

            CloseConnection();

            if (affectedRows > 0)
            {
                _idtypeevent = (int)idevent;
                _idtypeevent = idTypeEvent;
                _nameevent = nameEvent;
            }
        }

        public void UpdateEvent(int idEvent, int idTypeEvent, string nameEvent)
        {
            OpenConnection();
            string query = @"UPDATE Events 
                             SET IDTypeEvent = @IdTypeEvent, NameEvent = @NameEvent 
                             WHERE IdEvent = @IdEvent;";

            int affectedRows;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdEvent", idEvent);
                command.Parameters.AddWithValue("@IdTypeEvent", idTypeEvent);
                command.Parameters.AddWithValue("@NameEvent", nameEvent);

                affectedRows = command.ExecuteNonQuery();
            }
            CloseConnection();

            if (affectedRows > 0)
            {
                _idtypeevent = idEvent;
                _idtypeevent = idTypeEvent;
                _nameevent = nameEvent;
            }
        }

        public void DeleteEvent(int idEvent)
        {
            OpenConnection();
            string query = "DELETE FROM Events WHERE IdEvent = @IdEvent;";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdEvent", idEvent);

                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        public Dictionary<string, string> GetEventById(int idEvent)
        {
            OpenConnection();
            string query = "SELECT * FROM Events WHERE IdEvent = @IdEvent;";
            var result = new Dictionary<string, string>();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdEvent", idEvent);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result["IdEvent"] = reader.GetInt32(reader.GetOrdinal("IdEvent")).ToString();
                        result["IdTypeEvent"] = reader.GetInt32(reader.GetOrdinal("IdTypeEvent")).ToString();
                        result["NameEvent"] = reader["NameEvent"].ToString();
                    }
                }
            }
            CloseConnection();
            return result;
        }
        public List<Dictionary<string, string>> GetAllEvents()
        {
            OpenConnection();

            var EventsList = new List<Dictionary<string, string>>();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT IdEvent, IdTypeEvent, NameEvent FROM Events;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var EventsData = new Dictionary<string, string>
                        {
                            ["IdEvent"] = reader.GetInt32(reader.GetOrdinal("IdEvent")).ToString(),
                            ["IdTypeEvent"] = reader.GetInt32(reader.GetOrdinal("IdTypeEvent")).ToString(),
                            ["NameEvent"] = reader["NameEvent"].ToString(),
                        };
                        EventsList.Add(EventsData);
                    }
                }
            }

            CloseConnection();
            return EventsList;
        }
    }
}
