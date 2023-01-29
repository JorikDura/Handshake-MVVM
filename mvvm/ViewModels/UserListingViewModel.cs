using mvvm.Commands;
using mvvm.Models;
using mvvm.Services;
using mvvm.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace mvvm.ViewModels
{
    public class UserListingViewModel : ViewModelBase
    {
        private readonly UserStore _userStore;
        private readonly SqlService _sqlService;
        public ICommand DeleteUserCommand { get; }
        public ICommand NavigateUserCreationCommand { get; }
        private ObservableCollection<UserModel> _users;
        public ObservableCollection<UserModel> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }        

        public UserListingViewModel(UserStore userStore, SqlService sqlService, 
            NavigationStore navigationStore, Func<NavigationBarViewModel> navigationBarViewModel)
        {
            _userStore = userStore;
            _sqlService = sqlService;
            _users = new ObservableCollection<UserModel>();
            DeleteUserCommand = new BanUserCommand(this, _sqlService);
            INavigationService<CreateUserViewModel> navigationServiceCreate = new NavigationService<CreateUserViewModel>(
                navigationStore,
                () => new CreateUserViewModel(userStore, sqlService, navigationStore, navigationBarViewModel));
            NavigateUserCreationCommand = new NavigateCommand<CreateUserViewModel>(navigationServiceCreate);

            if (_userStore.CurrentUser.isAdmin)
                UpdateUserList();
        }
        /// <summary>
        /// обновляет список пользователей
        /// </summary>
        private void UpdateUserList()
        {
            _users.Clear();
            foreach(UserModel user in _sqlService.getAllUsers())
            {
                _users.Add(user);
            }
        }
    }
}
