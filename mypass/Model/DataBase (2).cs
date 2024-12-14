using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.Common;
using System.Data.SQLite;

using Microsoft.Data.Sqlite;

namespace mypass.Model
{
    // Класс для создания БД
    public static class DataBaseCreate
    {
        // Метод для создания новой базы данных с шифрованием
        public static void CreateEncryptedDatabase(string clientName, string password)
        {
            try
            {
                // Формирование пути для создания файла бд !! !!
                string databasePath = Path.Combine(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\"), $"{clientName}.db3");

                // Если база данных уже существует, удалить её
                if (File.Exists(databasePath))
                {
                    File.Delete(databasePath);
                }

                // Установить пароль шифрования
                using (var newConnection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
                {
                    newConnection.Open();

                    // Установить пароль шифрования
                    using (var command = newConnection.CreateCommand())
                    {
                        command.CommandText = $"PRAGMA key = '{password}';";
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Метод для создания всех таблиц бд
        public static void CreateTables(string clientName, string password)
        {
            try
            {
                // ЗАГЛУШКА
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    // Класс БД-шки
    public class DataBase
    {
        protected string _connectionString;
        protected SQLiteConnection _connection;
        private string _password;

        // Конструктор для инициализации строки подключения
        public DataBase(string databasePath, string password)
        {
            _connectionString = $"Data Source={databasePath};Version=3;";
            _password = password;
        }

        // Функция для открытия соединения
        public void OpenConnection()
        {
            if (_connection == null)
            {
                _connection = new SQLiteConnection(_connectionString);
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();

                // Установить пароль для доступа к зашифрованной БД
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = $"PRAGMA key = '{_password}';";
                    command.ExecuteNonQuery();
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
