using mypass.Model;

namespace mypass.ViewModel
{
    internal class RegistrationPageVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;
        public RegistrationPageVM()
        {
            _pageModel = new PageModel();
        }
    }
}
