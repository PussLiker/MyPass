using System;
using System.Collections.Generic;
using System.Data.SQLite;

// Зачем нужен: типо таблица с бд
// Наследование: лень писать
// Методы: лень писать
// Пример использования: лень писать

namespace mypass.Model
{
    public class TypeEventsDB : DataBase
    {
        private int _idtypeevent;
        public int IdTypeEvents
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

        public void AddTypeEvent(int IdTypeEvents, string TypeEvent)
        {
            OpenConnection();

            int affectedRows;

            string query = "INSERT INTO TypeEvents (TypeEvent) VALUES (@TypeEvent);";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdTypeEvent", IdTypeEvents);
                command.Parameters.AddWithValue("@TypeEvent", TypeEvent);

                affectedRows =command.ExecuteNonQuery();
            }
            CloseConnection();

            // Обновляем поля класса, если обновление прошло успешно
            if (affectedRows > 0)
            {
                _idtypeevent = IdTypeEvents;
                _typeevent = TypeEvent;
            }
        }

        public void UpdateTypeEvent(int idTypeEvent, string TypeEvent)
        {
            OpenConnection();

            int affectedRows;

            string query = @"UPDATE TypeEvents 
                             SET TypeEvent = @TypeEvent 
                             WHERE IdTypeEvent = @IdTypeEvent;";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdTypeEvent", idTypeEvent);
                command.Parameters.AddWithValue("@TypeEvent", TypeEvent);

                affectedRows = command.ExecuteNonQuery();
            }
            CloseConnection();

            if (affectedRows > 0)
            {
                _idtypeevent = IdTypeEvents;
                _typeevent = TypeEvent;
            }
        }

        public void DeleteTypeEvent(int idTypeEvent)
        {
            OpenConnection();

            int affectedRows;

            string query = "DELETE FROM TypeEvents WHERE IdTypeEvent = @IdTypeEvent;";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdTypeEvent", idTypeEvent);

                affectedRows = command.ExecuteNonQuery();
            }
            CloseConnection();

            if (affectedRows > 0)
            {
                _typeevent = "";
            }
        }

        public Dictionary<string, string> GetTypeEventById(int idTypeEvent)
        {
            OpenConnection();
            string query = "SELECT * FROM TypeEvents WHERE IdTypeEvent = @IdTypeEvent;";
            var result = new Dictionary<string, string>();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdTypeEvent", idTypeEvent);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        _idtypeevent = Convert.ToInt32(reader["IdTypeEvent"]);
                        _typeevent = reader["TypeEvent"].ToString();

                        result["IdTypeEvent"] = reader["IdTypeEvent"].ToString();
                        result["TypeEvent"] = reader["TypeEvent"].ToString();
                    }
                }
            }
            CloseConnection();
            return result;
        }

        public void LoadDataFromTypeEventsDB()
        {
            OpenConnection();

            string query = "SELECT IdTypeEvent, TypeEvent FROM TypeEvents;";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _idtypeevent = Convert.ToInt32(reader["IdTypeEvent"]);
                        _typeevent = reader["TypeEvent"].ToString();
                    }
                }
            }
            CloseConnection();
        }
    }
}
