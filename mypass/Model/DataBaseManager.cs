using System;
using System.IO;
using System.Data.SQLite;
using System.ComponentModel;

// Зачем нужен: этот класс нужен для управления созданием базы данных
// Наследование: предоставляет путь к базе данных (_databasePath)
// Методы: 'CreateDataBase' - для создания базы данных и инициализации таблиц

namespace mypass.Model
{
    public class DataBaseManager : Logging
    {
        public string _databasePath;
        protected string _databaseExtension = ".db";
        protected string _databaseName;

        // Метод для создания базы данных
        public bool CreateDataBase(string clientName) // Если возвращает false, то база уже существует
        {
            // Убедимся, что папка DataBase существует
            string targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase");
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            _databaseName = $"{clientName}{_databaseExtension}";
            _databasePath = Path.Combine(targetPath, _databaseName);
            History.addToHistory("База данных с именем " + clientName + " создана");
            if (!File.Exists(_databasePath))
            {
                SQLiteConnection.CreateFile(_databasePath);

                InitializeDatabase(); // Инициализация таблиц

                return true;
            }
            else
            {
                return false;
            }
        }

        // Метод для инициализации таблиц
        public void InitializeDatabase()
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={_databasePath};Version=3;"))
                {
                    connection.Open();

                    string[] tableCreationQueries = {
                        @"CREATE TABLE IF NOT EXISTS User (
                            LoginUser VARCHAR(128) PRIMARY KEY,
                            FirstName VARCHAR(128) NOT NULL,
                            SecondName VARCHAR(128) NOT NULL,
                            MasterPasswordHash VARCHAR(128) NOT NULL,
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
                        using (var command = new SQLiteCommand(query, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка инициализации базы данных: {ex.Message}");
            }
        }
    }
}
