using mvvm.Services;
using mvvm.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvvm.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        private readonly UserStore _userStore;
        private readonly SqlService _sqlService;
        private readonly NavigationStore _aboutViewNavigationService;

        public AboutViewModel(UserStore userStore, SqlService sqlService,
            NavigationStore aboutViewNavigationService)
        {
            _userStore = userStore;
            _sqlService = sqlService;
            _aboutViewNavigationService = aboutViewNavigationService;
        }
    }
}
