using mvvm.Models;
using mvvm.Services;
using mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvvm.Commands
{
    public class BanUserCommand : CommandBase
    {
        private readonly UserListingViewModel _userListingViewModel;
        private readonly SqlService _sqlService;

        public BanUserCommand(UserListingViewModel userListingViewModel, SqlService sqlService)
        {
            _userListingViewModel = userListingViewModel;
            _sqlService = sqlService;
        }
        public override void Execute(object parameter)
        {
            if(!parameter.Equals(null))
            {
                UserModel user = parameter as UserModel;
                if(_sqlService.banUser(user))
                    _userListingViewModel.Users.Remove(user);
                
            }
        }
    }
}
