using System;
using System.IO;

using System.Data.SQLite;

// Зачем нужен: этот класс нужен для упралвения созданием и установкой пароля для базы данных
// Наследование: идёт в наследование путь к созданной бд (_databasePath) и пароль пользователя (_passwordDB)
// Методы: 'CreateDataBase' - для создания БД, а также папки 'DataBase', если ранее не была создана и EncryptDataBase - для установки пользовательского пароля для бд

// Пример использования: DataBaseManager.CreateDataBase('Nikita');
//                       DataBaseManager.EncryptDataBase('230rt0450Tkkgji4');    - также можно вызывать много раз, тк с 59 по 62 строчку идёт проверка на уже имеющийся пароль

namespace mypass.Model
{
    public class DataBaseManager : Logging
    {
        // Переменные
        public string _databasePath;
        protected string _passwordDB;
        protected string _databaseExtension = ".bd";
        protected string _databaseName;


        // Метод для создания базы данных
        public bool CreateDataBase(string clientName, string password) // Если возвращает false, то надо вызывать методы для загрузки данных с БД, например
                                                                       // result = DataBaseManager.CreateDataBase(тут логин, тут пароль);
                                                                       // if result == false
                                                                       // {
                                                                       //     UserDB.LoadDataFromUserDB();
                                                                       //     TagsDB.LoadDataFromTagsDB();
                                                                       //     TypeEventsDB.LoadDataFromTypeEventsDB();
                                                                       //     TagsAccountsDB.LoadDataFromTagsAccountsDB();
                                                                       //     EventsDB.LoadDataFromEventsDB();
                                                                       //     ActionsDB.LoadDataFromActionsDB();
                                                                       //     AccountsDB.LoadDataFromAccountsDB();
                                                                       // }
        {
            // Путь для создания папки DataBase
            string targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase");
            Console.WriteLine(targetPath);
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            _databasePath = GetPathToDataBase(clientName);
            _databaseName = $"{clientName}{_databaseExtension}";

            Console.WriteLine(_databasePath);
            if (!File.Exists(_databasePath))
            {
                SQLiteConnection.CreateFile(_databasePath);

                //EncryptDataBase(password);

                InitializeDatabase(); // Инициализация таблиц

                return true;
            }
            else
            {
                return false;
            }
        }

       
        // Метод для шифрования базы данных
        public void EncryptDataBase(string password)
        {
            Console.WriteLine(_databasePath);
            if (string.IsNullOrEmpty(_databasePath) || !File.Exists(_databasePath))
            {
                throw new InvalidOperationException("База данных не существует. Сначала создайте базу данных.");
            }

            _passwordDB = password;

            using (var connection = new SQLiteConnection($"Data Source={_databasePath};Version=3;"))
            {
                Console.WriteLine(_databasePath);
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"PRAGMA key = '{_passwordDB}';";
                    command.ExecuteNonQuery();
                }
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
                using (var connection = new SQLiteConnection($"Data Source={_databasePath};Version=3;")) //Password={_passwordDB};
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
                    using (var command = new SQLiteCommand(query, connection))
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

                connection.Close();
                }
        }
            catch (Exception ex)
            {
                MessageError($"Ошибка инициализации базы данных: {ex.Message}");
    }
}
    }
}
