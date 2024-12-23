using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace mypass.Model
{
    // Класс для взаимодействия с базой данных
    public class DataBase : DataBaseManager
    {
        protected string _connectionString;
        protected SqliteConnection _connection;
        private readonly object _lock = new object();

        public DataBase()
        {
            // Формируем строку подключения к базе данных
            _connectionString = $"Data Source={_databasePath};";
            _connection = new SqliteConnection(_connectionString);
        }

        // Функция для открытия соединения
        public void OpenConnection()
        {
            
            if (_connection == null)
            {
                if (_connection == null)
                {
                    Console.WriteLine($"Инициализация подключения: {_connectionString}");
                    _connection = new SqliteConnection(_connectionString);
                }

                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    Console.WriteLine($"Открытие подключения: {_connectionString}");
                    _connection.Open();
                    Console.WriteLine("Подключение успешно открыто.");
                }
            }
        }

        // Функция для закрытия соединения
        public void CloseConnection()
        {
            lock (_lock)
            {
                if (_connection != null && _connection.State != System.Data.ConnectionState.Closed)
                {
                    _connection.Close();
                    Console.WriteLine("Подключение успешно закрыто.");
                }
            }
        }

        // Проверка состояния подключения
        public bool IsConnectionOpen()
        {
            return _connection != null && _connection.State == System.Data.ConnectionState.Open;
        }
    }
}
