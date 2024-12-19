using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace mypass.Model
{
    internal class EncryptionModel
    {
        //Создание массива байтов для ключа шифрования
        private static byte[] GenerateKey(string password)
        {

            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] fullKey = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));
                byte[] shortKey = new byte[32];
                Array.Copy(fullKey, shortKey, 32);
                return shortKey;
            }

        }
        //Генерация IV
        private static byte[] GenerateIV()
        {
            using (var aes = Aes.Create())
            {
                aes.GenerateIV();
                return aes.IV;
            }
        }
        //Шифрование пароля
        public static string Enscrypt(string password, string master_password)
        {
            byte[] key = GenerateKey(master_password);
            byte[] iv = GenerateIV();

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var enscryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var memoryStream = new MemoryStream())
                {
                    memoryStream.Write(iv, 0, iv.Length);

                    using (var cryptoStream = new CryptoStream(memoryStream, enscryptor, CryptoStreamMode.Write))
                    using (var writer = new StreamWriter(cryptoStream))
                    {
                        writer.Write(password);
                    }
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }
        //Дешифровка пароля
        public static string Decrypt(string password, string master_password)
        {
            byte[] key = GenerateKey(master_password);
            byte[] encryptPassword = Convert.FromBase64String(password);

            using (var memoryStream = new MemoryStream(encryptPassword))
            {
                byte[] iv = new byte[16];
                memoryStream.Read(iv, 0, iv.Length);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;

                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    using (var reader = new StreamReader(cryptoStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}

