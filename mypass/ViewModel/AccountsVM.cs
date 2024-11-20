using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mypass.Model;

namespace mypass.ViewModel
{
    class AccountsVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;
        
        //какие-то данные

        public AccountsVM() {
            _pageModel = new PageModel();
        }
    }
}
