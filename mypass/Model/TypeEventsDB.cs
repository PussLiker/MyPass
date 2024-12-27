using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Security.Policy;

namespace mypass.Model
{
    public class TypeEventsDB : DataBase
    {
        public TypeEventsDB(string databasePath)
        {
            _databasePath = databasePath;
            _connectionString = $"Data Source={_databasePath};Version=3;";
            _connection = new SQLiteConnection(_connectionString);
        }

        private int _idtypeevent;
        public int IdTypeEvent
        {
            get => _idtypeevent;
            set => _idtypeevent = value;
        }
        private string _typeevent;
        public string TypeEvent
        {
            get => _typeevent;
            set => _typeevent = value;
        }

        public void AddTypeEvent(string typeEvent)
        {
            OpenConnection();

            int affectedRows;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO TypeEvents (TypeEvent) VALUES (@TypeEvent);";
                command.Parameters.AddWithValue("@TypeEvent", typeEvent);

                affectedRows = command.ExecuteNonQuery();
            }

            long idtypeevent = _connection.LastInsertRowId;

            CloseConnection();

            if (affectedRows > 0)
            {
                _idtypeevent = (int)idtypeevent;
                _typeevent = typeEvent;
            }
        }

        public void UpdateTypeEvent(int idTypeEvent, string newTypeEvent)
        {
            OpenConnection();

            int affectedRows;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE TypeEvents SET TypeEvent = @TypeEvent WHERE IdTypeEvent = @IdTypeEvent;";
                command.Parameters.AddWithValue("@TypeEvent", newTypeEvent);
                command.Parameters.AddWithValue("@IdTypeEvent", idTypeEvent);

                affectedRows = command.ExecuteNonQuery();
            }

            CloseConnection();

            if (affectedRows > 0)
            {
                _idtypeevent = idTypeEvent;
                _typeevent = newTypeEvent;
            }
        }

        public void DeleteTypeEvent(int idTypeEvent)
        {
            OpenConnection();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM TypeEvents WHERE IdTypeEvent = @IdTypeEvent;";
                command.Parameters.AddWithValue("@IdTypeEvent", idTypeEvent);
            }

            CloseConnection();
        }

        public Dictionary<string, string> GetTypeEventById(int idTypeEvent)
        {
            OpenConnection();

            var result = new Dictionary<string, string>();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT IdTypeEvent, TypeEvent FROM TypeEvents WHERE IdTypeEvent = @IdTypeEvent;";
                command.Parameters.AddWithValue("@IdTypeEvent", idTypeEvent);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result["IdTypeEvent"] = reader.GetInt32(reader.GetOrdinal("IdTypeEvent")).ToString();
                        result["TypeEvent"] = reader["TypeEvent"].ToString();
                    }
                }
            }

            CloseConnection();
            return result;
        }

        public List<Dictionary<string, string>> GetAllTypeEvents()
        {
            OpenConnection();

            var TypeEventsList = new List<Dictionary<string, string>>();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT IdTypeEvent, TypeEvent FROM TypeEvents;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var TypeEventsData = new Dictionary<string, string>
                        {
                            ["IdTypeEvent"] = reader.GetInt32(reader.GetOrdinal("IdTypeEvent")).ToString(),
                            ["TypeEvent"] = reader["TypeEvent"].ToString(),
                        };
                        TypeEventsList.Add(TypeEventsData);
                    }
                }
            }

            CloseConnection();
            return TypeEventsList;
        }
    }
}
