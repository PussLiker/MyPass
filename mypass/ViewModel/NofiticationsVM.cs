using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mypass.Model;

namespace mypass.ViewModel 
{
    internal class NofiticationsVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public NofiticationsVM()
        {
            _pageModel = new PageModel();
        }
    }
}
