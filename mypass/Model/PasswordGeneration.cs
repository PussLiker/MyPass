using System;

namespace mypass.Model
{
    internal class PasswordGeneration
    {
        //Проверка доступности генерации
        public static bool CheckExecute(bool isNeedUpcase, bool isNeedLowercase,
    bool isNeedNumber, bool isNeedSpecialSymbol, string specialSymbols = null)
        {
            if (!isNeedUpcase && !isNeedLowercase && !isNeedNumber && !isNeedSpecialSymbol) return false;
            if (isNeedSpecialSymbol && !isNeedUpcase && !isNeedLowercase && !isNeedNumber && specialSymbols.Length < 2) return false;
            return true;
        }
        //Генерация пароля
        public static string PasswordGenerate(int size, bool isNeedUpcase, bool isNeedLowercase,
        bool isNeedNumber, bool isNeedSpecialSymbol, string specialSymbols = null)
        {

            const string upCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "1234567890";

            string masterString = "";
            if (isNeedUpcase) masterString += upCase;
            if (isNeedLowercase) masterString += lowerCase;
            if (isNeedNumber) masterString += numbers;
            if (isNeedSpecialSymbol) masterString += specialSymbols;

            string password = "";

            Random rand = new Random();

            if (isNeedUpcase) password += upCase[rand.Next(upCase.Length)];
            if (isNeedLowercase) password += lowerCase[rand.Next(lowerCase.Length)];
            if (isNeedNumber) password += numbers[rand.Next(numbers.Length)];
            if (isNeedSpecialSymbol) password += specialSymbols[rand.Next(specialSymbols.Length)];

            while (password.Length < size)
            {
                password += masterString[rand.Next(masterString.Length)];
            }
            History.addToHistory("Сгенерирован пароль");
            return Shuffle(password);
        }
        //Перемешка пароля
        private static string Shuffle(string password)
        {
            Random rand = new Random();
            char[] chars = password.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                int j = rand.Next(chars.Length);
                (chars[i], chars[j]) = (chars[j], chars[i]);
            }
            return new string(chars);
        }
    }

}


