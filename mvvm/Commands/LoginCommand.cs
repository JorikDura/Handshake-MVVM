using mvvm.Models;
using mvvm.Services;
using mvvm.Stores;
using mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace mvvm.Commands
{
    public class LoginCommand : CommandBase
    {
        private readonly SqlService _sqlService;
        private readonly UserStore _userStore;
        private readonly INavigationService<WelcomeViewModel> _navigationService;
        private readonly LoginViewModel _loginViewModel;

        public LoginCommand(LoginViewModel loginViewModel, SqlService sqlService, 
            UserStore userStore, INavigationService<WelcomeViewModel> navigationService)
        {
            _loginViewModel = loginViewModel;
            _sqlService = sqlService;
            _userStore = userStore;
            _navigationService = navigationService;
            _loginViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            if (!string.IsNullOrEmpty(_loginViewModel.Username) && 
                _loginViewModel.Password != null &&
                base.CanExecute(parameter))
                return true;
            return false;

        }
        public override void Execute(object parameter)
        {
            bool isValidUser = _sqlService.isAuthorized(new NetworkCredential(_loginViewModel.Username, _loginViewModel.Password));
            if (isValidUser)
            {
                UserModel userModel = _sqlService.getUserData(new NetworkCredential(_loginViewModel.Username, _loginViewModel.Password));
                if(userModel.isBanned)
                {
                    _loginViewModel.ErrorMessage = "Тебя забанили, друг";
                    return;
                }
                _userStore.CurrentUser = userModel;
                _navigationService.Navigate();
            }
            else
            {
                _loginViewModel.ErrorMessage = "Неправильный логин или пароль";
            }
        }
        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoginViewModel.Username) ||
                e.PropertyName == nameof(LoginViewModel.Password))
                OnCanExecuteChanged();
        }
    }
}
