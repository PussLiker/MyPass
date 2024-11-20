using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mypass.Model;

namespace mypass.ViewModel
{
    internal class CardsVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public CardsVM()
        {
            _pageModel = new PageModel();
        }
    }
}
