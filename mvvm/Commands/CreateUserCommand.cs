using Microsoft.Data.SqlClient;
using mvvm.Models;
using mvvm.Services;
using mvvm.Stores;
using mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace mvvm.Commands
{
    public class CreateUserCommand : CommandBase
    {
        private readonly CreateUserViewModel _createUserViewModel;
        private readonly SqlService _sqlService;
        private readonly INavigationService<UserListingViewModel> _navigationService;
        private readonly int _passwordMinSize;

        public CreateUserCommand(CreateUserViewModel createUserViewModel, 
            SqlService sqlService,
            INavigationService<UserListingViewModel> navigationService)
        {
            _createUserViewModel = createUserViewModel;
            _sqlService = sqlService;
            _navigationService = navigationService;
            _passwordMinSize = _sqlService.getPasswordSize();
            _createUserViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }
        public override bool CanExecute(object parameter)
        {
            return !string.IsNullOrWhiteSpace(_createUserViewModel.Name) &&
                !string.IsNullOrWhiteSpace(_createUserViewModel.Username) &&
                !string.IsNullOrWhiteSpace(_createUserViewModel.Phone) &&
                !string.IsNullOrWhiteSpace(_createUserViewModel.LastName) &&
                _createUserViewModel.Password != null;
        }
        public override void Execute(object parameter)
        {
            //Здесь делаем проверку на размер 
            if (_createUserViewModel.Password.Length > _passwordMinSize)
            {
                try
                {
                    NetworkCredential networkCredential = new NetworkCredential(_createUserViewModel.Username, _createUserViewModel.Password);
                    //шифрование пароля
                    PasswordCheckService.passwordEncrypt(ref networkCredential);
                    bool result = _sqlService.insertNewUser(
                        new NetworkCredential(_createUserViewModel.Username, _createUserViewModel.Password),
                        _createUserViewModel.Name, _createUserViewModel.LastName, _createUserViewModel.Phone);

                    if (result)
                        _navigationService.Navigate();

                    else
                        _createUserViewModel.ErrorMessage = "Чет не получилось";

                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)
                    {
                        _createUserViewModel.ErrorMessage = "Такой логин уже существует";
                    }
                }
            }
            //Если пароль меньше, выкидываем ошибку
            else
            {
                _createUserViewModel.ErrorMessage = $"Размера пароля должен быть больше {_passwordMinSize}";
            }
        }
        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CreateUserViewModel.Username) ||
                e.PropertyName == nameof(CreateUserViewModel.Password) ||
                e.PropertyName == nameof(CreateUserViewModel.Name) ||
                e.PropertyName == nameof(CreateUserViewModel.LastName) ||
                e.PropertyName == nameof(CreateUserViewModel.Phone))
                OnCanExecuteChanged();
        }
    }
}
