using System;
using System.Data.SQLite;
using System.IO;
using System.Web.UI.WebControls;

// Зачем нужен: этот класс служит родительским классом для всех классов таблиц. Тут основные методы всей БД
// Наследование: наследует от 'DataBaseManager' путь к файлу БД (_databasePath), а также пароль (_passwordDB)
// Методы: OpenConnection - метод нужен для открытия соединения с БД, всегда открывать перед началом транзакции
//         CloseConnection - метод нужен для закрытия соединения с БД

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
    }
}