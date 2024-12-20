using System;
using System.IO;

// Чтобы вызвать дебаг необходимо сделать его родительским классом необходимого для дебага класса
// Далее в начале метода необходимо инициализировать транзакцю, а в конце завершить
// Сообщения для отладки вызывать методом "MessageError"
// Чтобы работало необходимо  объявить о загрузке конфига примерно так: DebugConfig.LoadConfig()
// Подключать можно к любым файлам с расширением типа .cs
// Пример дебагинга можно увидеть в файле DataBase.cs, в классе DataBaseManager

namespace mypass.Model
{
    public abstract class LoggableDB
    {
        public StreamWriter _logWriter;

        // Метод для инициализации транзакции и начала логирования
        public void InitTransaction(string transactionName)
        {
            if (DebugConfig.IsDebugEnabled)
            {
                _logWriter = new StreamWriter(DebugConfig.LogFileName, true);
                _logWriter.WriteLine($"[{DateTime.Now}] Начало транзакции: {transactionName}");
                _logWriter.Flush();
            }
        }

        // Метод для записи ошибки в лог
        public void MessageError(string errorMessage)
        {
            if (DebugConfig.IsDebugEnabled && _logWriter != null)
            {
                _logWriter.WriteLine($"[{DateTime.Now}] Действие: {errorMessage}");
                _logWriter.Flush();
            }
        }

        // Метод для закрытия транзакции
        public void CloseTransaction(string message = "Транзакция завершена успешно")
        {
            if (DebugConfig.IsDebugEnabled && _logWriter != null)
            {
                _logWriter.WriteLine($"[{DateTime.Now}] {message}");
                _logWriter.WriteLine("-------------------------------------------");
                _logWriter.Flush();
                _logWriter.Close();
            }
        }
    }
}
