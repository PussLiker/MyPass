using mypass.Model;
﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace mypass.ViewModel
{
    internal class AllPassesVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;
        public ObservableCollection<Account> Accounts { get; set; }
        public AllPassesVM()
        {
            _pageModel = new PageModel();

            Accounts = new ObservableCollection<Account>
            {
            new Account { Username = "User1", Email = "user1@example.com", Password ="afskjhb"},
            new Account { Username = "User2", Email = "user2@example.com", Password ="afskjhb"},
            new Account { Username = "User1", Email = "user1@example.com", Password ="afskjhb"},
            new Account { Username = "User2", Email = "user2@example.com", Password ="afskjhb"},
            new Account { Username = "User1", Email = "user1@example.com", Password ="afskjhb"},
            new Account { Username = "User2", Email = "user2@example.com", Password ="afskjhb"},
            new Account { Username = "User1", Email = "user1@example.com", Password ="afskjhb"},
            new Account { Username = "User2", Email = "user2@example.com", Password ="afskjhb"},
            new Account { Username = "User1", Email = "user1@example.com", Password ="afskjhb"},
            new Account { Username = "User2", Email = "user2@example.com", Password ="afskjhb"},
            // ...
            };
        }
    }
}
