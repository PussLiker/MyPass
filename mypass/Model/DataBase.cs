using System;
using System.Data.SQLite;
using System.Web.UI.WebControls;

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
           
       
            _connectionString = $"Data Source={_databasePath};Version=3;";
            _connection = new SQLiteConnection(_connectionString);
            InitializeDatabase();


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
                FOREIGN KEY(LoginUserAccount) REFERENCES User(LoginUser) ON UPDATE CASCADE ON DELETE RESTRICT
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
        }
    }
}