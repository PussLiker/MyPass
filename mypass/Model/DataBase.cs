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
using System.Linq.Expressions;

namespace mypass.Model
{
    // Класс для создания БД
    public static class DataBaseManager
    {
        // Метод для создания новой базы данных с шифрованием
        public static void CreateEncryptedDatabase(string clientName, string password)
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
                    // Ставим пароль
                    command.CommandText = $"PRAGMA key = '{password}';";
                    command.ExecuteNonQuery();

                }
            }
        }
    }

    // Класс БД-шки
    public class DataBase
    {
        protected string _connectionString;
        protected SQLiteConnection _connection;
        private string _password;
        private string _databasePath;

        // Конструктор для инициализации строки подключения
        public DataBase(string databasePath, string password)
        {
            _connectionString = $"Data Source={databasePath};Version=3;";
            _password = password;
            _databasePath = databasePath;

            if (!File.Exists(_databasePath))
            {
                SQLiteConnection.CreateFile(_databasePath);
                InitializeDatabase(); // Инициализация таблиц при создании файла БД
            }

            string connectionString = $"Data Source={_databasePath};Version=3;Password={_password};";
            _connection = new SQLiteConnection(connectionString);
        }

        // Функция для открытия соединения
        public void OpenConnection()
        {

            if (_connection == null)
            {
                _connection = new SQLiteConnection(_connectionString);
            }

            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = $"PRAGMA key = '{_password}';";
                    command.ExecuteNonQuery();
                }

                // Проверить доступность базы данных после установки пароля
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT count(*) FROM sqlite_master;";
                    command.ExecuteScalar();
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
        private void InitializeDatabase()
        {
            OpenConnection();

            string createUsersTable = @"CREATE TABLE IF NOT EXISTS Users (
                IdUser INTEGER PRIMARY KEY AUTOINCREMENT,
                Username VARCHAR(128) NOT NULL,
                MasterPasswordHash VARCHAR(64) NOT NULL,
                Salt CHAR(16) NOT NULL
            );";

            string createAccountsTable = @"CREATE TABLE IF NOT EXISTS Accounts (
                IdAccount INTEGER PRIMARY KEY AUTOINCREMENT,
                IdUser INTEGER NOT NULL,
                ServiceName VARCHAR(128) NOT NULL,
                URL VARCHAR(255),
                Login VARCHAR(36),
                Password CHAR(64) NOT NULL,
                FOREIGN KEY(IdUser) REFERENCES Users(IdUser) ON UPDATE CASCADE ON DELETE RESTRICT
            );";

            string createTagsTable = @"CREATE TABLE IF NOT EXISTS Tags (
                IdTag INTEGER PRIMARY KEY AUTOINCREMENT,
                NameTag VARCHAR(36) NOT NULL
            );";

            string createTagsAccountsTable = @"CREATE TABLE IF NOT EXISTS TagsAccounts (
                IdTagsAccounts INTEGER PRIMARY KEY AUTOINCREMENT,
                IdAccount INTEGER NOT NULL,
                IdTag INTEGER NOT NULL,
                TimeTagging DATETIME NOT NULL,
                FOREIGN KEY(IdAccount) REFERENCES Accounts(IdAccount) ON UPDATE CASCADE ON DELETE RESTRICT,
                FOREIGN KEY(IdTag) REFERENCES Tags(IdTag) ON UPDATE CASCADE ON DELETE RESTRICT
            );";

            string createEventsTable = @"CREATE TABLE IF NOT EXISTS Events (
                IdEvent INTEGER PRIMARY KEY AUTOINCREMENT,
                IDTypeEvent INTEGER NOT NULL,
                NameEvent VARCHAR(128) NOT NULL,
                FOREIGN KEY(IDTypeEvent) REFERENCES TypeEvents(IdTypeEvent) ON UPDATE CASCADE ON DELETE RESTRICT
            );";

            string createActionsTable = @"CREATE TABLE IF NOT EXISTS Actions (
                IdAction INTEGER PRIMARY KEY AUTOINCREMENT,
                IdAccount INTEGER NOT NULL,
                IdEvent INTEGER NOT NULL,
                TimeEvent TIMESTAMP NOT NULL,
                FOREIGN KEY(IdAccount) REFERENCES Accounts(IdAccount) ON UPDATE CASCADE ON DELETE RESTRICT,
                FOREIGN KEY(IdEvent) REFERENCES Events(IdEvent) ON UPDATE CASCADE ON DELETE RESTRICT
            );";

            string createTypeEventsTable = @"CREATE TABLE IF NOT EXISTS TypeEvents (
                IdTypeEvent INTEGER PRIMARY KEY AUTOINCREMENT,
                TypeEvent VARCHAR(128) NOT NULL
            );";

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = createUsersTable;
                command.ExecuteNonQuery();

                command.CommandText = createAccountsTable;
                command.ExecuteNonQuery();

                command.CommandText = createTagsTable;
                command.ExecuteNonQuery();

                command.CommandText = createTagsAccountsTable;
                command.ExecuteNonQuery();

                command.CommandText = createEventsTable;
                command.ExecuteNonQuery();

                command.CommandText = createActionsTable;
                command.ExecuteNonQuery();

                command.CommandText = createTypeEventsTable;
                command.ExecuteNonQuery();
            }

            CloseConnection();
        }
    }
}

