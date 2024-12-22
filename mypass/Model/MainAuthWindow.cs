using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using mypass.ViewModel;

namespace mypass.Model
{
    internal class MainAuthWindow
    {
        private string _hz;

        public string HZ{
        get => _hz;
            
            set { 
            _hz = value;
            }
        }
        public void Registration(string Login, string Password, string Username, string UserSecondName) { 
           
            DataBaseManager DBM = new DataBaseManager();
            DBM.CreateDataBase(Login, Password);
            HZ = DBM._databasePath;
            UsersDB user = new UsersDB();
            Console.WriteLine(DBM._databasePath);
            Console.WriteLine(Password);
            user.AddUser(Login, Username, UserSecondName,  Password, "asdmkojashdfoashdasij");

        }
    }
}
