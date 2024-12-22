using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mypass.Model
{
    internal class Account
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        // Добавьте другие свойства по необходимости
        public void OpenLink()
        {
            Model.OpenLink.Open(Username);
        }
    }

}
