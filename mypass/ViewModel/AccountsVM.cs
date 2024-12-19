using mypass.Model;

namespace mypass.ViewModel
{
    class AccountsVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        //какие-то данные

        public AccountsVM()
        {
            _pageModel = new PageModel();
        }
    }
}
