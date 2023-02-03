using Microsoft.Data.SqlClient;
using mvvm.Models;
using mvvm.Services;
using mvvm.Stores;
using mvvm.ViewModels;
using System.ComponentModel;
using System.Linq;
using System.Net;

namespace mvvm.Commands
{
    public class RegistrationCommand : CommandBase
    {
        //Поля
        private readonly RegistrationViewModel _registrationViewModel;
        private readonly SqlService _sqlService;
        private readonly UserStore _userStore;
        private readonly INavigationService<WelcomeViewModel> _navigationService;
        private readonly int _passwordMinSize;

        public RegistrationCommand(RegistrationViewModel registrationViewModel, 
            SqlService sqlService, UserStore userStore,
            INavigationService<WelcomeViewModel> navigationService)
        {
            _registrationViewModel = registrationViewModel;
            _sqlService = sqlService;
            _userStore = userStore;
            _navigationService = navigationService;
            _registrationViewModel.PropertyChanged += OnViewModelPropertyChanged;
            _passwordMinSize = _sqlService.getPasswordSize();
        }
        /// <summary>
        /// Здесь делается валидация принятых данных
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool CanExecute(object parameter)
        {
            return !string.IsNullOrWhiteSpace(_registrationViewModel.Username) &&
                !string.IsNullOrWhiteSpace(_registrationViewModel.Name) &&
                !string.IsNullOrWhiteSpace(_registrationViewModel.LastName) &&
                !string.IsNullOrWhiteSpace(_registrationViewModel.Phone) &&
                _registrationViewModel.Password != null;
        }
        /// <summary>
        /// Выполнение команды - регистрация
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            //Здесь делаем проверку на размер 
            if(_registrationViewModel.Password.Length > _passwordMinSize)
            {
                NetworkCredential networkCredential = 
                    new NetworkCredential(_registrationViewModel.Username, _registrationViewModel.Password);
                
                //шифруем пароль
                try
                {
                    PasswordCheckService.passwordEncrypt(ref networkCredential);
                }
                catch
                {
                    _registrationViewModel.ErrorMessage = "Что-то пошло не так.";
                    return;
                }

                //проверка на символы в пароле
                if (_sqlService.getIsPresence())
                {
                    if (PasswordCheckService.checkPassword(networkCredential.Password))
                        this.Registration(networkCredential);

                    else
                        _registrationViewModel.ErrorMessage = "Пароль должен содержать спец. символы";
                }

                else
                    this.Registration(networkCredential);
            }
            //Если пароль меньше, выкидываем ошибку
            else
            {
                _registrationViewModel.ErrorMessage = $"Размера пароля должен быть больше {_passwordMinSize}";
            }
        }
        /// <summary>
        /// Специальное событие, происходящее при изменении компонентов wpf (TextBox)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(RegistrationViewModel.Username) ||
                e.PropertyName == nameof(RegistrationViewModel.Password) ||
                e.PropertyName == nameof(RegistrationViewModel.Name) ||
                e.PropertyName == nameof(RegistrationViewModel.LastName) ||
                e.PropertyName == nameof(RegistrationViewModel.Phone))
                OnCanExecuteChanged();
        }

        private void Registration(NetworkCredential networkCredential)
        {
            try
            {
                bool result = _sqlService.insertNewUser(networkCredential,
            _registrationViewModel.Name, _registrationViewModel.LastName, _registrationViewModel.Phone);

                if (result)
                {
                    UserModel userModel =
                        _sqlService.getUserData(
                        new NetworkCredential(_registrationViewModel.Username, _registrationViewModel.Password));
                    _userStore.CurrentUser = userModel;
                    _navigationService.Navigate();
                }

                else
                {
                    _registrationViewModel.ErrorMessage = "Чет не получилось";
                }

            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    _registrationViewModel.ErrorMessage = "Такой логин уже существует";
                }
            }
        }
    }
}
