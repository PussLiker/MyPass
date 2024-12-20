using System.Collections.Generic;
using System.Data.SQLite;

namespace mypass.Model
{
    public class TypeEventsDB : DataBase
    {
        private int _id;
        public int ID
        {
            get => _id;
            set => _id = value;
        }
        private string _typeevent;
        public string TypeEvent
        {
            get => _typeevent;
            set => _typeevent = value;
        }
        public TypeEventsDB(string databasePath, string password) : base() { }

        public void AddTypeEvent(string typeEvent)
        {
            OpenConnection();
            string query = "INSERT INTO TypeEvents (TypeEvent) VALUES (@TypeEvent);";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@TypeEvent", typeEvent);

                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        public void UpdateTypeEvent(int idTypeEvent, string typeEvent)
        {
            OpenConnection();
            string query = @"UPDATE TypeEvents 
                             SET TypeEvent = @TypeEvent 
                             WHERE IdTypeEvent = @IdTypeEvent;";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdTypeEvent", idTypeEvent);
                command.Parameters.AddWithValue("@TypeEvent", typeEvent);

                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        public void DeleteTypeEvent(int idTypeEvent)
        {
            OpenConnection();
            string query = "DELETE FROM TypeEvents WHERE IdTypeEvent = @IdTypeEvent;";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdTypeEvent", idTypeEvent);

                command.ExecuteNonQuery();
            }
            CloseConnection();
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
                        result["IdTypeEvent"] = reader["IdTypeEvent"].ToString();
                        result["TypeEvent"] = reader["TypeEvent"].ToString();
                    }
                }
            }
            CloseConnection();
            return result;
        }
    }
}
