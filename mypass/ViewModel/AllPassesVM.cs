using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mypass.Model;

namespace mypass.ViewModel
{
    internal class AllPassesVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public AllPassesVM()
        {
            _pageModel = new PageModel();
        }
    }
}
