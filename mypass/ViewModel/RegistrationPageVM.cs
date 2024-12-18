using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mypass.Model;
using mypass.Utilities;
using System.Windows.Controls;
using System.Windows.Input;

namespace mypass.ViewModel
{
    internal class RegistrationPageVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;              
        public RegistrationPageVM() {
            _pageModel = new PageModel();
        }
    }
}
