using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvvm.Services
{
    public class PasswordCheck
    {
        /// <summary>
        /// Проверяет пароль на наличие чисел и мат. символов
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool checkPassword(string password)
        {
            string[] list = new[] { "~", "`", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "+", "=", "\"" };
            return password.Any(char.IsDigit) && list.Contains(password);
        }
    }
}
