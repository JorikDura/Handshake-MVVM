using mvvm.Stores;
using mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvvm.Services
{
    public class ParameterNavigationService<TParametr, TViewModel>
        where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TParametr, TViewModel> _createViewModel;

        public ParameterNavigationService(NavigationStore navigationStore, Func<TParametr, TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate(TParametr parametr)
        {
            _navigationStore.CurrentViewModel = _createViewModel(parametr);
        }
    }
}
