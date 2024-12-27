using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace mypass.Model
{
    internal class PasswordHasher
    {
        // Метод для генерации соли
        public static string GenerateSalt()
        {
            string salt = PasswordGeneration.PasswordGenerate(16,true,true,true, false);
            return salt;
        }

        // Метод для создания хэша с солью
        public static string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + salt;
                var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
                var hashBytes = sha256.ComputeHash(saltedPasswordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        // Метод для проверки пароля
        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            var enteredHash = HashPassword(enteredPassword, storedSalt);
            return storedHash == enteredHash;
        }
    }
}
