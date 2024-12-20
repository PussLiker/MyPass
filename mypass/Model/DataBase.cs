using System;
using System.Data.SQLite;

// Зачем нужен: этот класс служит родительским классом для всех классов таблиц. Тут основные методы всей БД
// Наследование: наследует от 'DataBaseManager' путь к файлу БД (_databasePath), а также пароль (_passwordDB)
// Методы: OpenConnection - метод нужен для открытия соединения с БД, всегда открывать перед началом транзакции
//         CloseConnection - метод нужен для закрытия соединения с БД
//         InitializeDatabase - метод нужен для создания всех таблиц в БД

// Пример использования: хз, потом напишу, мне лень



namespace mypass.Model
{
    // Класс для взаимодействия с базой данных
    public class DataBase : DataBaseManager
    {
        protected string _connectionString;
        protected SQLiteConnection _connection;

        public DataBase()
        {
            InitTransaction("Вызов базы данных для заполнения атрибутов");
            if (string.IsNullOrEmpty(_databasePath) || string.IsNullOrEmpty(_passwordDB))
            {
                MessageError("Ошибка: отстутствует путь и шифрование БД");
                throw new InvalidOperationException("Необходимо сначала создать и зашифровать базу данных.");
            }

            _connectionString = $"Data Source={_databasePath};Version=3;Password={_passwordDB};";
            _connection = new SQLiteConnection(_connectionString);
            CloseTransaction("Завершение изменения значений");
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
                MessageError("Установлено соединение с БД");
            }
        }

        // Функция для закрытия соединения
        public void CloseConnection()
        {
            if (_connection != null && _connection.State != System.Data.ConnectionState.Closed)
            {
                _connection.Close();
                MessageError("Соединение с БД закрыто");
            }
        }
        private void InitializeDatabase()
        {
            InitTransaction("Вызов базы данных для создания таблиц");
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
                MessageError("Создание таблицы 'Users'");
                command.CommandText = createUsersTable;
                command.ExecuteNonQuery();

                MessageError("Создание таблицы 'Accounts'");
                command.CommandText = createAccountsTable;
                command.ExecuteNonQuery();

                MessageError("Создание таблицы 'Tags'");
                command.CommandText = createTagsTable;
                command.ExecuteNonQuery();

                MessageError("Создание таблицы 'TagsAccounts'");
                command.CommandText = createTagsAccountsTable;
                command.ExecuteNonQuery();

                MessageError("Создание таблицы 'Events");
                command.CommandText = createEventsTable;
                command.ExecuteNonQuery();

                MessageError("Создание таблицы 'Actions'");
                command.CommandText = createActionsTable;
                command.ExecuteNonQuery();

                MessageError("Создание таблицы 'TypeEvents'");
                command.CommandText = createTypeEventsTable;
                command.ExecuteNonQuery();
            }

            CloseConnection();
            CloseTransaction("Завершение создания таблиц");
        }
    }
}