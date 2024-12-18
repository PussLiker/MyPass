using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mypass.View;
using mypass.Utilities;

namespace mypass.ViewModel
{
    internal class VhodVM : ViewModelBase
    {
        private object _vhodvm;

        public object VhodimVM
        {
            get { return _vhodvm; }
            set { _vhodvm = value; OnPropertyChanged(); }

        }
        private string _pass;
        private string _log;

        public string Log
        {
            get => _log;
            set 
            {
                if (_pass != value)
                { _log = value; OnPropertyChanged(); }
            }
        }
        public string Pass
        {
            get => _pass;
            set
            {
                if (_pass != value)
                {
                    _pass = value;
                    OnPropertyChanged(nameof(Pass)); // Уведомляем View об изменении
                }
            }
        }

    }
}
