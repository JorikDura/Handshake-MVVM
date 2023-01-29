using mvvm.Services;
using mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvvm.Commands
{
    public class ChangePresencePassword : CommandBase
    {
        private readonly OptionsViewModel _optionsViewModel;
        private readonly SqlService _sqlService;

        public ChangePresencePassword(OptionsViewModel optionsViewModel, SqlService sqlService)
        {
            _optionsViewModel = optionsViewModel;
            _sqlService = sqlService;
        }

        public override void Execute(object parameter)
        {
            if (_optionsViewModel.CheckBox)
                _sqlService.changePresence(_optionsViewModel.CheckBox);
            else
                _sqlService.changePresence(_optionsViewModel.CheckBox);
        }
    }
}
