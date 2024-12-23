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
