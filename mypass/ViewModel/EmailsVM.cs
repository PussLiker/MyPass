using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mypass.Model;

namespace mypass.ViewModel
{
    internal class EmailsVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public EmailsVM()
        {
            _pageModel = new PageModel();
        }
    }
}
