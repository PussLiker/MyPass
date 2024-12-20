using System;

using System.Data.SQLite;

namespace mypass.Model
{
    // Класс для взаимодействия с базой данных
    public class DataBase : DataBaseManager
    {
        protected string _connectionString;
        protected SQLiteConnection _connection;

        public DataBase()
        {
            if (string.IsNullOrEmpty(_databasePath) || string.IsNullOrEmpty(_passwordDB))
            {
                throw new InvalidOperationException("Необходимо сначала создать и зашифровать базу данных.");
            }

            _connectionString = $"Data Source={_databasePath};Version=3;Password={_passwordDB};";
            _connection = new SQLiteConnection(_connectionString);
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