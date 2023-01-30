using mvvm.Services;
using mvvm.Stores;
using mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace mvvm.Commands
{
    public class PasswordResetCommand : CommandBase
    {
        private readonly OptionsViewModel _optionsViewModel;
        private readonly SqlService _sqlService;
        private readonly UserStore _userStore;

        public PasswordResetCommand(OptionsViewModel optionsViewModel, SqlService sqlService, UserStore userStore)
        {
            _optionsViewModel = optionsViewModel;
            _sqlService = sqlService;
            _userStore = userStore;
        }

        public override void Execute(object parameter)
        {
            NetworkCredential network = new NetworkCredential(null, _optionsViewModel.NewPassword);
            if(_sqlService.getIsPresence())
            {
                if (PasswordCheckService.checkPassword(network.Password))
                {
                    _sqlService.changePassword(_optionsViewModel.Id, new NetworkCredential(null, _optionsViewModel.NewPassword));
                    _optionsViewModel.PasswordStatus = "Успешно!";
                    return;
                }
                else
                {
                    _optionsViewModel.PasswordStatus = "Нет спец. символов";
                    return;
                }
            }
            _sqlService.changePassword(_optionsViewModel.Id, new NetworkCredential(null, _optionsViewModel.NewPassword));
            _optionsViewModel.PasswordStatus = "Успешно!";
        }
    }
}
