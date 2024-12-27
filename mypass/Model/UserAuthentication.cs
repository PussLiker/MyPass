using System;
using System.IO;
using System.Data.SQLite;
using mypass.View;

public class UserAuthentication
{
    private string _databaseFolderPath; // Путь к папке с базами данных

    public UserAuthentication(string DatabaseFolderPath)
    {
        _databaseFolderPath = DatabaseFolderPath;
    }

    public bool Authenticate(string login, string password)
    {
        // Формируем полный путь к базе данных
        string databaseFilePath = Path.Combine(_databaseFolderPath, $"{login}.db");

        // Проверяем, существует ли файл
        if (!File.Exists(databaseFilePath))
        {
            ErrorWindow.ShowError("Пользователь с таким логином не найден.");
            return false;
        }

        // Подключаемся к базе данных и проверяем пароль
        using (SQLiteConnection connection = new SQLiteConnection($"Data Source={databaseFilePath};Version=3;"))
        {
            try
            {
                connection.Open();

                string query = "SELECT MasterPasswordHash FROM User WHERE LoginUser = @Login";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);

                    var result = command.ExecuteScalar();

                    if (result != null)
                    {
                        string storedPassword = result.ToString();
                        if (storedPassword == password)
                        {
                            
                            return true;
                        }
                        else
                        {
                            ErrorWindow.ShowError("Неверный пароль.");
                            return false;
                        }
                    }
                    else
                    {
                        ErrorWindow.ShowError("Пользователь не найден в базе данных.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorWindow.ShowError($"Ошибка при проверке пароля: {ex.Message}");
                return false;
            }
        }
    }
}
