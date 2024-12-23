using System;
using System.IO;
<<<<<<< Updated upstream

using System.Data.SQLite;

// Зачем нужен: этот класс нужен для упралвения созданием и установкой пароля для базы данных
// Наследование: идёт в наследование путь к созданной бд (_databasePath) и пароль пользователя (_passwordDB)
// Методы: 'CreateDataBase' - для создания БД, а также папки 'DataBase', если ранее не была создана и EncryptDataBase - для установки пользовательского пароля для бд

// Пример использования: DataBaseManager.CreateDataBase('Nikita');
//                       DataBaseManager.EncryptDataBase('230rt0450Tkkgji4'); - также можно вызывать много раз, тк с 59 по 62 строчку идёт проверка на уже имеющийся пароль
=======
using Microsoft.Data.Sqlite;
>>>>>>> Stashed changes

namespace mypass.Model
{
    public class DataBaseManager : Logging
    {
        // Переменные
<<<<<<< Updated upstream
        protected string _databasePath;
        protected string _passwordDB;
=======
        public string _databasePath;
        protected string _databaseExtension = ".db";
        protected string _databaseName;
        protected string _passwordDataBase;
>>>>>>> Stashed changes

        // Метод для создания базы данных
        public bool CreateDataBase(string clientName, string password)
        {
            // Логирование
            InitTransaction("Создание базы данных");

            // Путь для создания папки DataBase
<<<<<<< Updated upstream
            string targetPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\", "DataBase"));
            MessageError($"Создан путь для создания папки DataBase: {targetPath}");

=======
            string targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase");
>>>>>>> Stashed changes
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
                MessageError("Создана папка DataBase");
            }

<<<<<<< Updated upstream
            // Финальный путь для создания файла
            string databaseExtension = ".sqlite";
            string databaseName = $"{clientName}.{databaseExtension}";
            _databasePath = Path.Combine(targetPath, databaseName);
            MessageError($"Создан путь к БД: {_databasePath}");

            if (!File.Exists(_databasePath))
            {
                SQLiteConnection.CreateFile(_databasePath);
                MessageError($"База данных создана: {databaseName}");
                CloseTransaction("Создание базы данных завершено");

                EncryptDataBase(password);

=======
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
>>>>>>> Stashed changes
                return true;
            }
            else
            {
                MessageError($"База данных уже существует: {databaseName}");
                CloseTransaction();

                return false;
            }
        }

<<<<<<< Updated upstream
        // Метод для шифрования базы данных
        public void EncryptDataBase(string password)
        {
            if (string.IsNullOrEmpty(_databasePath) || !File.Exists(_databasePath))
            {
                throw new InvalidOperationException("База данных не существует. Сначала создайте базу данных.");
            }

            _passwordDB = password;

            InitTransaction("Шифрование базы данных");
            using (var connection = new SQLiteConnection($"Data Source={_databasePath};Version=3;"))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"PRAGMA key = '{_passwordDB}';";
                    command.ExecuteNonQuery();
                    MessageError("Пароль успешно установлен для базы данных");
                }
            }
            CloseTransaction("Шифрование базы данных завершено");
=======
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
                                MessageError($"Таблица успешно создана: {query.Substring(0, 30)}...");
                            }
                            catch (Exception ex)
                            {
                                MessageError($"Ошибка создания таблицы: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageError($"Ошибка инициализации базы данных: {ex.Message}");
            }
>>>>>>> Stashed changes
        }
    }
}
