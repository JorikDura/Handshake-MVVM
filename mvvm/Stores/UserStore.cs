using mvvm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvvm.Stores
{
    public class UserStore
    {
        private UserModel _currentUser;
        public UserModel CurrentUser { get; set; }


    }
}
