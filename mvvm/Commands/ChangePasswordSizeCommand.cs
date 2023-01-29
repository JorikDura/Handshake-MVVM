using mvvm.Services;
using mvvm.Stores;
using mvvm.ViewModels;
using System;

namespace mvvm.Commands
{
    public class ChangePasswordSizeCommand : CommandBase
    {
        private readonly OptionsViewModel _optionsViewModel;
        private readonly SqlService _sqlService;
        private readonly UserStore _userStore;

        public ChangePasswordSizeCommand(OptionsViewModel optionsViewModel, SqlService sqlService, 
            UserStore userStore)
        {
            _optionsViewModel = optionsViewModel;
            _sqlService = sqlService;
            _userStore = userStore;
        }
        public override bool CanExecute(object parameter)
        {
            if (_userStore.CurrentUser.isAdmin)
                return true;
            return false;
        }
        public override void Execute(object parameter)
        {
            _sqlService.changePasswordSize(_optionsViewModel.PasswordMinSize);
            _optionsViewModel.StatusMessage = "Успешно!";
        }
    }
}
