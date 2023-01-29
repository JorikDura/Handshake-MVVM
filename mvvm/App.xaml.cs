using mvvm.Services;
using mvvm.Stores;
using mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace mvvm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private SqlService _sqlService;
        private readonly NavigationStore _navigationStore;
        private readonly UserStore _userStore;
        public App()
        {
            _sqlService = new SqlService();
            _navigationStore = new NavigationStore();
            _userStore = new UserStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            INavigationService<LoginViewModel> loginNavigationService = CreateLoginVeiwModel();
            loginNavigationService.Navigate();

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        private INavigationService<LoginViewModel> CreateLoginVeiwModel()
        {
            return new NavigationService<LoginViewModel>(_navigationStore, 
                () => new LoginViewModel(_userStore, _sqlService, _navigationStore, CreateNavigationBarViewModel));
        }
        private INavigationService<RegistrationViewModel> CreateRegistrationViewModel()
        {
            return new NavigationService<RegistrationViewModel>(_navigationStore,
                () => new RegistrationViewModel(_userStore, _sqlService, _navigationStore, CreateNavigationBarViewModel));
        }
        private INavigationService<UserListingViewModel> CreateUserListingNavigationModel()
        {
            return new LayoutNavigationService<UserListingViewModel>(_navigationStore, 
                () => new UserListingViewModel(_userStore, _sqlService, _navigationStore, CreateNavigationBarViewModel),
                CreateNavigationBarViewModel);
        }
        private INavigationService<AboutViewModel> CreateAboutNavigationModel()
        {
            return new LayoutNavigationService<AboutViewModel>(_navigationStore, 
                () => new AboutViewModel(),
                CreateNavigationBarViewModel);
        }
        private INavigationService<OptionsViewModel> CreateOptionsNavigationModel()
        {
            return new LayoutNavigationService<OptionsViewModel>(_navigationStore,
                () => new OptionsViewModel(_userStore, _sqlService, _navigationStore),
                CreateNavigationBarViewModel);
        }

        private NavigationBarViewModel CreateNavigationBarViewModel()
        {
            return new NavigationBarViewModel(_userStore,
                CreateUserListingNavigationModel(),
                CreateOptionsNavigationModel(),
                CreateAboutNavigationModel()
                );
        }
    }
}
