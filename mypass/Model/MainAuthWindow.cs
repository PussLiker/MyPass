using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mypass.ViewModel;

namespace mypass.Model
{
    internal class MainAuthWindow
    {

        public void Registration(string Login, string Password, string Username, string UserSecondName) { 
           
            DataBaseManager DBM = new DataBaseManager();
            DBM.CreateDataBase(Login);
            DBM.EncryptDataBase(Password);
            UsersDB user = new UsersDB();
            user.AddUser(1, Username, UserSecondName, Login, Password, "asdmkojashdfoashdasij");
        }
    }
}
