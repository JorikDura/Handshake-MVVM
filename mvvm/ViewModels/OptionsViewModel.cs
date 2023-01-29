using mvvm.Commands;
using mvvm.Services;
using mvvm.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace mvvm.ViewModels
{
    public class OptionsViewModel : ViewModelBase
    {
        private readonly UserStore _userStore;
        private readonly SqlService _sqlService;
        private readonly NavigationStore _optionsViewNavigationService;
        private int _passwordMinSize;
        public int PasswordMinSize
        {
            get
            {
                return _passwordMinSize;
            }
            set
            {
                _passwordMinSize = Convert.ToInt32(value);
                OnPropertyChanged(nameof(PasswordMinSize));
            }
        }
        private string _statusMessage;
        public string StatusMessage
        {
            get
            {
                return _statusMessage;
            }
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }
        private SecureString _newPassword;
        public SecureString NewPassword
        {
            get
            {
                return _newPassword;
            }
            set
            {
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
            }
        }
        private string _passwordStatus;
        public string PasswordStatus
        {
            get
            {
                return _passwordStatus;
            }
            set
            {
                _passwordStatus = value;
                OnPropertyChanged(nameof(PasswordStatus));
            }
        }
        private bool _isPasswordReset;
        public bool IsPasswordReset
        {
            get
            {
                return _isPasswordReset;
            }
            set
            {
                _isPasswordReset = value;
                OnPropertyChanged(nameof( IsPasswordReset));
            }
        }
        private bool _checkBox;
        public bool CheckBox
        {
            get
            {
                return _checkBox;
            }
            set
            {
                _checkBox = value;
                OnPropertyChanged(nameof(CheckBox));
            }
        }
        public int Id => _userStore.CurrentUser.Id;
        public string Username => _userStore.CurrentUser.Username;
        public string Name => _userStore.CurrentUser.Name;
        public string LastName => _userStore.CurrentUser.LastName;
        public string Phone => _userStore.CurrentUser.Phone;
        public bool IsAdminLoggedIn => _userStore.CurrentUser.isAdmin;
        public ICommand PasswordMinSizeChangeCommand { get; }
        public ICommand PasswordResetCommand { get; }
        public ICommand WantResetPasswordCommand { get; }
        public ICommand PasswordPresenceNumberCommand { get; }

        public OptionsViewModel(UserStore userStore, SqlService sqlService, 
            NavigationStore optionsViewNavigationService)
        {
            _userStore = userStore;
            _sqlService = sqlService;
            _optionsViewNavigationService = optionsViewNavigationService;
            PasswordMinSize = _sqlService.getPasswordSize();
            CheckBox = _sqlService.getIsPresence();
            PasswordMinSizeChangeCommand = new ChangePasswordSizeCommand(this, sqlService, userStore);
            PasswordResetCommand = new PasswordResetCommand(this, sqlService, userStore);
            WantResetPasswordCommand = new WantResetPasswordCommand(this);
            PasswordPresenceNumberCommand = new ChangePresencePassword(this, sqlService);
        }
    }
}
