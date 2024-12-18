using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mypass.Model
{
    internal class PasswordValidator
    { 
      // Метод проверки надёжности пароля
        public static PasswordCheckResult CheckPasswordStrength(string password)
        {
            int score = 0;
            PasswordCheckResult result = new PasswordCheckResult();

            // Проверка на минимальную длину
            if (password.Length < 8)
            {
                result.Status = "very_weak";
                result.Message = "Пароль слишком короткий. Минимум 8 символов.";
                result.Color = "red"; // Красный для очень слабого пароля
                return result;
            }
            else if (password.Length >= 8)
                score += 1; // Базовый балл за длину

            // Длина пароля
            if (password.Length >= 12)
                score += 1;
            else if (password.Length >= 16)
                score += 3;
            else if (password.Length >= 30)
                

            // Разнообразие символов
            if (Regex.IsMatch(password, @"\d")) // Наличие цифр
                score += 1;
            if (Regex.IsMatch(password, @"[a-z]")) // Наличие строчных букв
                score += 1;
            if (Regex.IsMatch(password, @"[A-Z]")) // Наличие заглавных букв
                score += 1;
            if (Regex.IsMatch(password, @"[!~@#$%^&*(),.?""':;{}|<>_\-\[\]\\+=]")) // Наличие спецсимволов
                score += 2;

            // Проверка на повторяющиеся символы или последовательности
            if (Regex.IsMatch(password, @"(.)\1{2,}")) // Повтор одного символа 3 и более раз
                score -= 1;

            if (IsSequential(password)) // Последовательности, типа "1234" или "abcd"
                score -= 2;

            // Проверка на распространённые пароли
            if (CommonPasswordsList.Passwords.Contains(password.ToLower()))
            {
                result.Status = "very_weak";
                result.Message = "Очень слабый пароль: входит в список распространённых паролей.";
                result.Color = "red"; // Красный для очень слабого пароля
                return result;
            }

            // Результат на основе баллов
            if (score <= 2)
            {
                result.Status = "very_weak";
                result.Message = "Очень слабый пароль";
                result.Color = "red"; // Красный для очень слабого пароля
            }
            else if (score <= 4)
            {
                result.Status = "weak";
                result.Message = "Слабый пароль";
                result.Color = "orange"; // Оранжевый для слабого пароля
            }
            else if (score <= 6)
            {
                result.Status = "medium";
                result.Message = "Средний пароль";
                result.Color = "yellow"; // Желтый для среднего пароля
            }
            else if (score <= 8)
            {
                result.Status = "strong";
                result.Message = "Хороший пароль";
                result.Color = "green"; // Зеленый для хорошего пароля
            }
            else
            {
                result.Status = "very_strong";
                result.Message = "Очень надёжный пароль";
                result.Color = "green"; // Зеленый для очень сильного пароля
            }

            return result;
        }

        // Проверка на последовательности (например, "1234", "abcd")
        private static bool IsSequential(string password)
        {
            char[] chars = password.ToCharArray();

            for (int i = 0; i < chars.Length - 2; i++)
            {
                if ((chars[i + 1] == chars[i] + 1) && (chars[i + 2] == chars[i] + 2))
                    return true;

                if ((chars[i + 1] == chars[i] - 1) && (chars[i + 2] == chars[i] - 2))
                    return true;
            }

            return false;
        }
    }
}