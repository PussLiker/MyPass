using System;
using System.Collections.Generic;
using System.IO;
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
        
        public bool Registration(string Login, string Password, string Username, string UserSecondName) {

            var DB = new DataBase();
            if (DB.CreateDataBase(Login) == true)
            {
                var user = new UsersDB(DB._databasePath);
                string salt = PasswordHasher.GenerateSalt();
                user.AddUser(Login, Username, UserSecondName, PasswordHasher.HashPassword(Password, salt), salt);
                PageModel.login = Login;
                PageModel.masterPassword = Password;
                return true;
            }
            else
            {
                ErrorWindow.ShowError("Такой пользователь уже существует!");
                return false;
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

        static string dbpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase");
        public string DBpath = dbpath;
        UserAuthentication UserAuth = new UserAuthentication(dbpath);

        public bool Vhodim(string Login, string Password)
        {            
            if (UserAuth.Authenticate(Login, Password))
            {
                PageModel.login = Login;
                PageModel.masterPassword = Password;
                return true;
            }
            return false;
        }
    }
}
