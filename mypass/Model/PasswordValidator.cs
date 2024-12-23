using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace mypass.Model
{
    internal class PasswordValidator
    {
        public enum PasswordStrength
        {
            VeryWeak,
            Weak,
            Medium,
            Strong
        }

        // Метод проверки с детализацией
        public static (PasswordStrength strength, List<string> feedback) CheckPasswordStrength(string password)
        {
            

            List<string> feedback = new List<string>();
            int score = 0;

            if (CommonPasswordsList.Passwords.Contains(password.ToLower()))
            {
                return (PasswordStrength.VeryWeak, new List<string> { "Этот пароль слишком распространён. Выберите другой." });
            }

            // Минимальная длина
            if (password.Length < 12)
            {
                feedback.Add("Пароль слишком короткий. Минимальная длина: 12 символов.");
                return (PasswordStrength.VeryWeak, feedback);
            }
            else
                score++;

            // Длина пароля
            if (password.Length >= 25) score += 3;
            if (password.Length >= 14) score += 2;
            if (password.Length >= 16) score += 2;

            // Цифры
            if (Regex.IsMatch(password, @"\d"))
                score++;
            else
                feedback.Add("Добавьте хотя бы одну цифру.");

            // Строчные буквы
            if (Regex.IsMatch(password, @"[a-z]"))
                score++;
            else
                feedback.Add("Добавьте хотя бы одну строчную букву.");

            // Заглавные буквы
            if (Regex.IsMatch(password, @"[A-Z]"))
                score++;
            else
                feedback.Add("Добавьте хотя бы одну заглавную букву.");

            // Специальные символы
            if (Regex.IsMatch(password, @"[!~@#$%^&*(),.?""':;{}|<>_\-\[\]\\+=]"))
                score++;
            else
                feedback.Add("Добавьте хотя бы один специальный символ.");

            // Повторяющиеся символы
            if (Regex.IsMatch(password, @"(.)\1{2,}"))
                feedback.Add("Избегайте повторяющихся символов подряд."); score -= 2;
                
            // Последовательности
            if (IsSequential(password))
                feedback.Add("Избегайте последовательностей вроде '1234' или 'abcd'."); score--;

            // Оценка по баллам
            PasswordStrength strength;

            if (score <= 2)
            {
                strength = PasswordStrength.VeryWeak;
            }
            else if (score <= 4)
            {
                strength = PasswordStrength.Weak;
            }
            else if (score <= 5)
            {
                strength = PasswordStrength.Medium;
            }
            else
            {
                strength = PasswordStrength.Strong;
            }

            return (strength, feedback);
        }


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
