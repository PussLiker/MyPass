using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using mypass.View;
using mypass.ViewModel;

namespace mypass.Model
{
    internal class MainAuthWindowClass
    {
  
        public void Registration(string Login, string Password, string Username, string UserSecondName) {

            var DB = new DataBase();
            DB.CreateDataBase(Login);
            if (DB.CreateDataBase(Login) == true)
            {
                var user = new UsersDB(DB._databasePath);
                user.AddUser(Login, Username, UserSecondName, Password, "asdmkojashdfoashdasij");
            }
            else
            {

            }
        }

        public bool  GdePole(string pole1)
        {
            if(pole1 == null) return false;
            else return true;
        }
        public int IsPasswordNorm(string Password, string PasswordUnconf)
        {
            if (Password != null && PasswordUnconf != null)
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

        public void PassNotNull() 
        {
            ErrorWindow.ShowError("Пароль не может быть пустым!");
        }
        public void PassNotEquel()
        {
            ErrorWindow.ShowError("Пароли не одинаковые!");
        }
        public void GdePolaNull()
        {
            ErrorWindow.ShowError("Имя, Фамилия и Логин должны быть заполнены!");
        }
    }
}
