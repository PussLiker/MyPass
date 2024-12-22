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
        protected string _databasePath;
        protected string _passwordDB;

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
            // Логирование
            InitTransaction("Создание базы данных");

            // Путь для создания папки DataBase
            string targetPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\", "DataBase"));
            MessageError($"Создан путь для создания папки DataBase: {targetPath}");

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
                MessageError("Создана папка DataBase");
            }

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

                return true;
            }
            else
            {
                MessageError($"База данных уже существует: {databaseName}");
                CloseTransaction();

                return false;
            }
        }

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
        }
    }
}
