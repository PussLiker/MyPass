using System;
using System.IO;

using Microsoft.Data.Sqlite;


namespace mypass.Model
{
    public class DataBaseManager : Logging
    {
        // Переменные

        public string _databasePath;
        protected string _databaseExtension = ".db";
        protected string _databaseName;
        protected string _passwordDataBase;

        // Метод для создания базы данных
        public bool CreateDataBase(string clientName, string password)
        {
            // Логирование
            InitTransaction("Создание базы данных");

            // Путь для создания папки DataBase

            string targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase");

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
                MessageError("Создана папка DataBase");
            }

            // Путь к базе данных
            _databaseName = clientName;
            _databasePath = GetPathToDataBase(clientName);
            _passwordDataBase = password;

            if (!File.Exists(_databasePath))
            {
                // Создание базы данных
                using (var connection = new SqliteConnection($"Data Source={_databasePath}"))
                {
                    connection.Open();
                    InitializeDatabase(); // Инициализация таблиц
                    connection.Close();
                }

                return true;
            }
            else
            {
                MessageError($"База данных уже существует: {_databaseName}");
                CloseTransaction();

                return false;
            }
        }
        public string GetPathToDataBase(string clientName)
        {
            string targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase");
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            string databaseName = $"{clientName}{_databaseExtension}";
            return Path.Combine(targetPath, databaseName);
        }

        // Метод для инициализации таблиц
        public void InitializeDatabase()
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={_databasePath}"))
                {
                    connection.Open();

                    string[] tableCreationQueries = {
                        @"CREATE TABLE IF NOT EXISTS User (
                            LoginUser VARCHAR(128) PRIMARY KEY,
                            FirstName VARCHAR(128) NOT NULL,
                            SecondName VARCHAR(128) NOT NULL,
                            MasterPasswordHash VARCHAR(64) NOT NULL,
                            Salt CHAR(16) NOT NULL
                        );",

                        @"CREATE TABLE IF NOT EXISTS Tags (
                            IdTag INTEGER PRIMARY KEY AUTOINCREMENT,
                            NameTag VARCHAR(36) NOT NULL
                        );",

                        @"CREATE TABLE IF NOT EXISTS TypeEvents (
                            IdTypeEvent INTEGER PRIMARY KEY AUTOINCREMENT,
                            TypeEvent VARCHAR(128) NOT NULL
                        );",

                        @"CREATE TABLE IF NOT EXISTS Accounts (
                            IdAccount INTEGER PRIMARY KEY AUTOINCREMENT,
                            LoginUserAccount VARCHAR(128) NOT NULL,
                            ServiceName VARCHAR(128) NOT NULL,
                            URL VARCHAR(255),
                            LoginAccount VARCHAR(36),
                            Password CHAR(64) NOT NULL,
                            FOREIGN KEY(LoginUserAccount) REFERENCES User(LoginUser) ON UPDATE CASCADE ON DELETE RESTRICT
                        );",

                        @"CREATE TABLE IF NOT EXISTS Events (
                            IdEvent INTEGER PRIMARY KEY AUTOINCREMENT,
                            IDTypeEvent INTEGER NOT NULL,
                            NameEvent VARCHAR(128) NOT NULL,
                            FOREIGN KEY(IDTypeEvent) REFERENCES TypeEvents(IdTypeEvent) ON UPDATE CASCADE ON DELETE RESTRICT
                        );",

                        @"CREATE TABLE IF NOT EXISTS TagsAccounts (
                            IdTagsAccounts INTEGER PRIMARY KEY AUTOINCREMENT,
                            IdAccount INTEGER NOT NULL,
                            IdTag INTEGER NOT NULL,
                            TimeTagging DATETIME NOT NULL,
                            FOREIGN KEY(IdAccount) REFERENCES Accounts(IdAccount) ON UPDATE CASCADE ON DELETE RESTRICT,
                            FOREIGN KEY(IdTag) REFERENCES Tags(IdTag) ON UPDATE CASCADE ON DELETE RESTRICT
                        );",

                        @"CREATE TABLE IF NOT EXISTS Actions (
                            IdAction INTEGER PRIMARY KEY AUTOINCREMENT,
                            IdAccount INTEGER NOT NULL,
                            IdEvent INTEGER NOT NULL,
                            TimeEvent TIMESTAMP NOT NULL,
                            FOREIGN KEY(IdAccount) REFERENCES Accounts(IdAccount) ON UPDATE CASCADE ON DELETE RESTRICT,
                            FOREIGN KEY(IdEvent) REFERENCES Events(IdEvent) ON UPDATE CASCADE ON DELETE RESTRICT
                        );"
                    };

                    foreach (var query in tableCreationQueries)
                    {
                        using (var command = new SqliteCommand(query, connection))
                        {
                            try
                            {
                                command.ExecuteNonQuery();

                            }
                            catch (Exception)
                            {
                                //
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //
            }
        }
    }
}
