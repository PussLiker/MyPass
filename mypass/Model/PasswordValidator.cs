using System.Text.RegularExpressions;

namespace mypass.Model
{
    internal class PasswordValidator
    {
        // Перечисление для определения уровня надёжности пароля
        public enum PasswordStrength
        {
            VeryWeak,
            Weak,
            Medium,
            Strong,
            VeryStrong
        }

        // Метод проверки надёжности пароля
        public static PasswordStrength CheckPasswordStrength(string password)
        {
            // Проверка на распространённые пароли
            if (CommonPasswordsList.Passwords.Contains(password.ToLower()))
            {
                return PasswordStrength.VeryWeak;
            }

            int score = 0;

            // Проверка на минимальную длину
            if (password.Length < 8)
            {
                return PasswordStrength.VeryWeak;
            }
            else if (password.Length >= 8)
                score += 1; // Базовый балл за длину

            // Длина пароля
            if (password.Length >= 30)
                return PasswordStrength.VeryStrong;
            else if (password.Length >= 12)
                score += 1;
            else if (password.Length >= 16)
                score += 3;


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

            // Результат на основе баллов
            if (score <= 2)
            {
                return PasswordStrength.VeryWeak;
            }
            else if (score <= 4)
            {
                return PasswordStrength.Weak;
            }
            else if (score <= 6)
            {
                return PasswordStrength.Medium;
            }
            else if (score <= 8)
            {
                return PasswordStrength.Strong;
            }
            else
            {
                return PasswordStrength.VeryStrong;
            }
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
