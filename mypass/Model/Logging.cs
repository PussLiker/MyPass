using System;
using System.IO;

// Зачем нужен: Этот класс нужен для упралвения методами логирования
// Наследование: косвенно получает значения из класса 'DebugConfig.cs': 'IsDebugEnabled' и 'LogFileName'
// Методы: 'InitTransaction' - метод для начала транзакции логирования. Нужен для логического отделения логов
//         'MessageError' - метод для ввода обычного сообщения в логи
//         'CloseTransaction' - метод для логического завершения транзакции.

// Пример использования: какой-то код
//                       InitTransaction("Создание БД");
//                       какой-то код
//                       MessageError("Ваш текст для логов");
//                       какой-то код
//                       CloseTransaction("Завершение создания БД"); - необязательно указывать значение, тк есть значение по умолчанию


namespace mypass.Model
{
    public abstract class Logging
    {
        private StreamWriter _logWriter;

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
