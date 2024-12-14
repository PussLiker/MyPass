using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mypass.Model;

namespace mypass.ViewModel
{
    internal class PassCheckVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public PassCheckVM()
        {
            _pageModel = new PageModel();
        }
    }
}
