using mvvm.Commands;
using mvvm.Services;
using mvvm.Stores;
using System;
using System.Security;
using System.Windows.Input;

namespace mvvm.ViewModels
{
    public class CreateUserViewModel : ViewModelBase
    {
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
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        private string _phone;
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
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
        private readonly SqlService _sqlService;
        private readonly NavigationStore _navigationStore;
        public ICommand CreateUserCommand { get; }
        public ICommand NavigateToUserListingCommand { get; }

        public CreateUserViewModel(UserStore userStore, SqlService sqlService, 
            NavigationStore navigationStore, Func<NavigationBarViewModel> navigationBarViewModel)
        {
            _sqlService = sqlService;
            _navigationStore = navigationStore;
            INavigationService<UserListingViewModel> navigationServiceUserList = new LayoutNavigationService<UserListingViewModel>(
                navigationStore,
                () => new UserListingViewModel(userStore, sqlService, navigationStore, navigationBarViewModel), navigationBarViewModel);
            CreateUserCommand = new CreateUserCommand(this, sqlService, navigationServiceUserList);
            NavigateToUserListingCommand = new NavigateCommand<UserListingViewModel>(navigationServiceUserList);
        }
    }
}
