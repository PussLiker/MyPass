using System;
using System.IO;

using System.Data.SQLite;

namespace mypass.Model
{
    public class DataBaseManager : LoggableDB
    {
        // Переменные
        protected string _databasePath;
        protected string _passwordDB;

        // Метод для создания базы данных
        public void CreateDataBase(string clientName)
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
            string databaseExtension = "db3";
            string databaseName = $"{clientName}.{databaseExtension}";
            _databasePath = Path.Combine(targetPath, databaseName);
            MessageError($"Создан путь к БД: {_databasePath}");

            if (!File.Exists(_databasePath))
            {
                SQLiteConnection.CreateFile(_databasePath);
                MessageError($"База данных создана: {databaseName}");
            }
            else
            {
                MessageError($"База данных уже существует: {databaseName}");
            }

            CloseTransaction("Создание базы данных завершено");
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
