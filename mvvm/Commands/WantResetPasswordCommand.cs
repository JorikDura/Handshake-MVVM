using mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvvm.Commands
{
    public class WantResetPasswordCommand : CommandBase
    {
        private readonly OptionsViewModel _optionsViewModel;

        public WantResetPasswordCommand(OptionsViewModel optionsViewModel)
        {
            _optionsViewModel = optionsViewModel;
        }

        public override void Execute(object parameter)
        {
            if(_optionsViewModel.IsPasswordReset)
                _optionsViewModel.IsPasswordReset = false;
            else
                _optionsViewModel.IsPasswordReset = true;
        }
    }
}
