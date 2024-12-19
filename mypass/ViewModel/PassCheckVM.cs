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
