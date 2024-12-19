using mypass.Model;

namespace mypass.ViewModel
{
    internal class EmailsVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public EmailsVM()
        {
            _pageModel = new PageModel();
        }
    }
}
