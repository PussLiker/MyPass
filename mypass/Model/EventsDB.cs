using System.Collections.Generic;

namespace mypass.Model
{
    public class EventsDB : DataBase
    {
        public EventsDB(string databasePath, string password) : base(databasePath, password) { }

        public void AddEvent(int idTypeEvent, string nameEvent)
        {
            OpenConnection();
            string query = "INSERT INTO Events (IDTypeEvent, NameEvent) VALUES (@IDTypeEvent, @NameEvent);";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IDTypeEvent", idTypeEvent);
                command.Parameters.AddWithValue("@NameEvent", nameEvent);

                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        public void UpdateEvent(int idEvent, int idTypeEvent, string nameEvent)
        {
            OpenConnection();
            string query = @"UPDATE Events 
                             SET IDTypeEvent = @IDTypeEvent, NameEvent = @NameEvent 
                             WHERE IdEvent = @IdEvent;";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@IdEvent", idEvent);
                command.Parameters.AddWithValue("@IDTypeEvent", idTypeEvent);
                command.Parameters.AddWithValue("@NameEvent", nameEvent);

                command.ExecuteNonQuery();
            }
            CloseConnection();
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
                        result["IdEvent"] = reader["IdEvent"].ToString();
                        result["IDTypeEvent"] = reader["IDTypeEvent"].ToString();
                        result["NameEvent"] = reader["NameEvent"].ToString();
                    }
                }
            }
            CloseConnection();
            return result;
        }
    }
}
