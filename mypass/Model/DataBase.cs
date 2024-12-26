using System;
using System.Data.SQLite;

// Зачем нужен: этот класс служит родительским классом для всех классов таблиц
// Методы: OpenConnection - открывает соединение с БД
//         CloseConnection - закрывает соединение с БД

namespace mypass.Model
{
    public class DataBase : DataBaseManager
    {
        protected string _connectionString;
        protected SQLiteConnection _connection;

        public DataBase()
        {
            _connectionString = $"Data Source={_databasePath};Version=3;";
            _connection = new SQLiteConnection(_connectionString);
        }

        private readonly object _lock = new object();

        // Функция для открытия соединения
        public void OpenConnection()
        {
            lock (_lock)
            {
                if (_connection == null)
                {
                    _connection = new SQLiteConnection(_connectionString);
                }

                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    _connection.Open();
                }
            }
        }

        // Функция для закрытия соединения
        public void CloseConnection()
        {
            if (_connection != null && _connection.State != System.Data.ConnectionState.Closed)
            {
                _connection.Close();
            }
        }
    }
}
