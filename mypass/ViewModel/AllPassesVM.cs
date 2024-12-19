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
