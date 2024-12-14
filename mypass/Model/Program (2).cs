using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;
using System.Globalization;

namespace mypass.Model
{
    class Program
    {
        static void Programer(string[] args)
        {
            // Создать базу данных для клиента
            string clientName = "Dima";
            string password = "securepassword123";

            DataBaseCreate.CreateEncryptedDatabase(clientName, password);
        }
    }
}