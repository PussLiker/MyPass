<<<<<<< Updated upstream
﻿using System;
using System.Data.SQLite;

// Зачем нужен: этот класс служит родительским классом для всех классов таблиц. Тут основные методы всей БД
// Наследование: наследует от 'DataBaseManager' путь к файлу БД (_databasePath), а также пароль (_passwordDB)
// Методы: OpenConnection - метод нужен для открытия соединения с БД, всегда открывать перед началом транзакции
//         CloseConnection - метод нужен для закрытия соединения с БД
//         InitializeDatabase - метод нужен для создания всех таблиц в БД

// Пример использования: хз, потом напишу, мне лень
=======
﻿using Microsoft.Data.Sqlite;
using System;
using System.IO;
>>>>>>> Stashed changes



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
<<<<<<< Updated upstream
            InitTransaction("Вызов базы данных для заполнения атрибутов");
            if (string.IsNullOrEmpty(_databasePath) || string.IsNullOrEmpty(_passwordDB))
            {
                MessageError("Ошибка: отстутствует путь и шифрование БД");
                throw new InvalidOperationException("Необходимо сначала создать и зашифровать базу данных.");
            }

            _connectionString = $"Data Source={_databasePath};Version=3;Password={_passwordDB};";
            _connection = new SQLiteConnection(_connectionString);
            CloseTransaction("Завершение изменения значений");
=======
            // Формируем строку подключения к базе данных
            _connectionString = $"Data Source={_databasePath};";
            _connection = new SqliteConnection(_connectionString);
>>>>>>> Stashed changes
        }

        // Функция для открытия соединения
        public void OpenConnection()
        {
            
            if (_connection == null)
            {
<<<<<<< Updated upstream
                _connection = new SQLiteConnection(_connectionString);
            }

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
                MessageError("Установлено соединение с БД");
=======
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
>>>>>>> Stashed changes
            }
        }

        // Функция для закрытия соединения
        public void CloseConnection()
        {
            lock (_lock)
            {
<<<<<<< Updated upstream
                _connection.Close();
                MessageError("Соединение с БД закрыто");
            }
        }
        private void InitializeDatabase()
        {
            InitTransaction("Вызов базы данных для создания таблиц");
            OpenConnection();

            // 1
            string createUsersTable = @"CREATE TABLE IF NOT EXISTS User (
                LoginUser VARCHAR(128) PRIMARY KEY,
                FirstName VARCHAR(128) NOT NULL,
                SecondName VARCHAR(128) NOT NULL,
                MasterPasswordHash VARCHAR(64) NOT NULL,
                Salt CHAR(16) NOT NULL
            );";
            
            // 2
            string createTagsTable = @"CREATE TABLE IF NOT EXISTS Tags (
                IdTag INTEGER PRIMARY KEY AUTOINCREMENT,
                NameTag VARCHAR(36) NOT NULL
            );";

            // 3
            string createTypeEventsTable = @"CREATE TABLE IF NOT EXISTS TypeEvents (
                IdTypeEvent INTEGER PRIMARY KEY AUTOINCREMENT,
                TypeEvent VARCHAR(128) NOT NULL
            );";

            // 4
            string createAccountsTable = @"CREATE TABLE IF NOT EXISTS Accounts (
                IdAccount INTEGER PRIMARY KEY AUTOINCREMENT,
                LoginUserAccount VARCHAR(128) NOT NULL,
                ServiceName VARCHAR(128) NOT NULL,
                URL VARCHAR(255),
                LoginAccount VARCHAR(36),
                Password CHAR(64) NOT NULL,
                FOREIGN KEY(Login) REFERENCES User(Login) ON UPDATE CASCADE ON DELETE RESTRICT
            );";

            // 5
            string createEventsTable = @"CREATE TABLE IF NOT EXISTS Events (
                IdEvent INTEGER PRIMARY KEY AUTOINCREMENT,
                IDTypeEvent INTEGER NOT NULL,
                NameEvent VARCHAR(128) NOT NULL,
                FOREIGN KEY(IDTypeEvent) REFERENCES TypeEvents(IdTypeEvent) ON UPDATE CASCADE ON DELETE RESTRICT
            );";

            // 6
            string createTagsAccountsTable = @"CREATE TABLE IF NOT EXISTS TagsAccounts (
                IdTagsAccounts INTEGER PRIMARY KEY AUTOINCREMENT,
                IdAccount INTEGER NOT NULL,
                IdTag INTEGER NOT NULL,
                TimeTagging DATETIME NOT NULL,
                FOREIGN KEY(IdAccount) REFERENCES Accounts(IdAccount) ON UPDATE CASCADE ON DELETE RESTRICT,
                FOREIGN KEY(IdTag) REFERENCES Tags(IdTag) ON UPDATE CASCADE ON DELETE RESTRICT
            );";

            // 7
            string createActionsTable = @"CREATE TABLE IF NOT EXISTS Actions (
                IdAction INTEGER PRIMARY KEY AUTOINCREMENT,
                IdAccount INTEGER NOT NULL,
                IdEvent INTEGER NOT NULL,
                TimeEvent TIMESTAMP NOT NULL,
                FOREIGN KEY(IdAccount) REFERENCES Accounts(IdAccount) ON UPDATE CASCADE ON DELETE RESTRICT,
                FOREIGN KEY(IdEvent) REFERENCES Events(IdEvent) ON UPDATE CASCADE ON DELETE RESTRICT
            );";

            using (var command = _connection.CreateCommand())
            {
                MessageError("Создание таблицы 'Users'");
                command.CommandText = createUsersTable;
                command.ExecuteNonQuery();

                MessageError("Создание таблицы 'Tags'");
                command.CommandText = createTagsTable;
                command.ExecuteNonQuery();

                MessageError("Создание таблицы 'TypeEvents'");
                command.CommandText = createTypeEventsTable;
                command.ExecuteNonQuery();

                MessageError("Создание таблицы 'Accounts'");
                command.CommandText = createAccountsTable;
                command.ExecuteNonQuery();

                MessageError("Создание таблицы 'Events");
                command.CommandText = createEventsTable;
                command.ExecuteNonQuery();

                MessageError("Создание таблицы 'TagsAccounts'");
                command.CommandText = createTagsAccountsTable;
                command.ExecuteNonQuery();

                MessageError("Создание таблицы 'Actions'");
                command.CommandText = createActionsTable;
                command.ExecuteNonQuery();
            }
            CloseConnection();
            CloseTransaction("Завершение создания таблиц");
=======
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
>>>>>>> Stashed changes
        }
    }
}
