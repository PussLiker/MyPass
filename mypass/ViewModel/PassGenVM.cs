using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mypass.Model;

namespace mypass.ViewModel
{
    internal class PassGenVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public PassGenVM()
        {
            _pageModel = new PageModel();
        }
    }
}
