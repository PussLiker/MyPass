using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using mypass.View;
using System.Windows.Controls;
using System.Windows.Input;
using mypass.Model;
using mypass.Utilities;

namespace mypass.ViewModel
{
    internal class VhodVM : ViewModelBase
    {

        private readonly PageModel _pageModel;
        public VhodVM() {
            _pageModel = new PageModel();   

            
        }

    }
}
    