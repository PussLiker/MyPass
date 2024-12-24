using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using mypass.ViewModel;

namespace mypass.Model
{
    internal class MainAuthWindowClass
    {

        public void Registration(string Login, string Password, string Username, string UserSecondName) {

            var DB = new DataBase();
            DB.CreateDataBase(Login);

            var user = new UsersDB(DB._databasePath);
            user.AddUser(Login, Username, UserSecondName, "asdmkojashdfoashdasij");

        }

        public bool  GdePole(string pole1)
        {
            if(pole1 == null) return false;
            else return true;
        }
        public int IsPasswordNorm(string Password, string PasswordUnconf)
        {
            if (Password != null || PasswordUnconf != null)
            {
                if (Password.Equals(PasswordUnconf))
                {

                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 3;
            }
        }
    }
}
