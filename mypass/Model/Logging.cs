using System;
using System.IO;

namespace mypass.Model
{
    public abstract class LoggableDB
    {
        private StreamWriter _logWriter;

        // Метод для инициализации транзакции и начала логирования
        protected void InitTransaction(string transactionName)
        {
            if (DebugConfig.IsDebugEnabled)
            {
                _logWriter = new StreamWriter(DebugConfig.LogFileName, true);
                _logWriter.WriteLine($"[{DateTime.Now}] Начало транзакции: {transactionName}");
                _logWriter.Flush();
            }
        }

        // Метод для записи ошибки в лог
        protected void MessageError(string errorMessage)
        {
            if (DebugConfig.IsDebugEnabled && _logWriter != null)
            {
                _logWriter.WriteLine($"[{DateTime.Now}] Действие: {errorMessage}");
                _logWriter.Flush();
            }
        }

        // Метод для закрытия транзакции
        protected void CloseTransaction(string message = "Транзакция завершена успешно")
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
