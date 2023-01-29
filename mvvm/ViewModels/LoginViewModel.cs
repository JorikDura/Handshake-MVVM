using mvvm.Commands;
using mvvm.Models;
using mvvm.Services;
using mvvm.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace mvvm.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly SqlService _sqlService;
		private readonly NavigationStore _parameterNavigationService;
		private string _username;
		public string Username
		{
			get
			{
				return _username;
			}
			set
			{
				_username = value;
				OnPropertyChanged(nameof(Username));
			}
		}

		private SecureString _password;
		public SecureString Password
		{
			get
			{
				return _password;
			}
			set
			{
                _password = value;
				OnPropertyChanged(nameof(Password));
			}
		}
		private string _errorMessage;
		public string ErrorMessage
		{
			get
			{
				return _errorMessage;
			}
			set
			{
                _errorMessage = value;
				OnPropertyChanged(nameof(ErrorMessage));
			}
		}

		public ICommand LoginCommand { get; }
        public ICommand NavigateToRegistrationCommand { get; }

		public LoginViewModel(UserStore userStore, SqlService sqlService, 
			NavigationStore navigationStore, Func<NavigationBarViewModel> navigationBarViewModel)
		{
			_sqlService = sqlService;
			_parameterNavigationService = navigationStore;
            INavigationService<WelcomeViewModel> navigationService = new LayoutNavigationService<WelcomeViewModel>(
				navigationStore,
				() => new WelcomeViewModel(), navigationBarViewModel);
            INavigationService<RegistrationViewModel> navigationServiceReg = new NavigationService<RegistrationViewModel>(
				navigationStore,
				() => new RegistrationViewModel(userStore, sqlService, navigationStore, navigationBarViewModel));
			LoginCommand = new LoginCommand(this, _sqlService, userStore, navigationService);
			NavigateToRegistrationCommand = new NavigateCommand<RegistrationViewModel>(navigationServiceReg);
		}
    }
}
