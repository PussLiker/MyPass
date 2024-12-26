using System;
using System.Collections.Generic;
using System.Data.SQLite;

// Зачем нужен: таблица для хранения типов событий
// Наследование: от DataBase для работы с SQLite
// Методы: работа с типами событий (добавление, обновление, удаление, получение)

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

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO TypeEvents (TypeEvent) VALUES (@TypeEvent);";
                command.Parameters.AddWithValue("@TypeEvent", typeEvent);

                if (command.ExecuteNonQuery() > 0)
                {
                    TypeEvent = typeEvent;
                }
            }

            CloseConnection();
        }

        public void UpdateTypeEvent(int idTypeEvent, string newTypeEvent)
        {
            OpenConnection();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE TypeEvents SET TypeEvent = @TypeEvent WHERE IdTypeEvent = @IdTypeEvent;";
                command.Parameters.AddWithValue("@TypeEvent", newTypeEvent);
                command.Parameters.AddWithValue("@IdTypeEvent", idTypeEvent);

                if (command.ExecuteNonQuery() > 0)
                {
                    IdTypeEvent = idTypeEvent;
                    TypeEvent = newTypeEvent;
                }
            }

            CloseConnection();
        }

        public void DeleteTypeEvent(int idTypeEvent)
        {
            OpenConnection();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM TypeEvents WHERE IdTypeEvent = @IdTypeEvent;";
                command.Parameters.AddWithValue("@IdTypeEvent", idTypeEvent);

                if (command.ExecuteNonQuery() > 0)
                {
                    IdTypeEvent = 0;
                    TypeEvent = null;
                }
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
                        IdTypeEvent = Convert.ToInt32(reader["IdTypeEvent"]);
                        TypeEvent = reader["TypeEvent"].ToString();

                        result["IdTypeEvent"] = IdTypeEvent.ToString();
                        result["TypeEvent"] = TypeEvent;
                    }
                }
            }

            CloseConnection();
            return result;
        }

        public List<Dictionary<string, string>> GetAllTypeEvents()
        {
            OpenConnection();

            var result = new List<Dictionary<string, string>>();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT IdTypeEvent, TypeEvent FROM TypeEvents;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Dictionary<string, string>
                        {
                            { "IdTypeEvent", reader["IdTypeEvent"].ToString() },
                            { "TypeEvent", reader["TypeEvent"].ToString() }
                        });
                    }
                }
            }

            CloseConnection();
            return result;
        }
    }
}
