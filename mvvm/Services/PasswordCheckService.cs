using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace mvvm.Services
{
    public class PasswordCheckService
    {
        //поле ключ и список символов для пароля
        private static int[] key;
        private static string[] symbolList = new[] { "~", "`", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "+", "=", "\"" };
        private static int countSybol = 0;
        /// <summary>
        /// Проверяет пароль на наличие чисел и мат. символов
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool checkPassword(string password)
        {
            return password.Any(char.IsDigit) && symbolList.Any(password.Contains);
        }
        /// <summary>
        /// Шифрует пароль
        /// </summary>
        /// <param name="input"></param>
        public static void passwordEncrypt(ref NetworkCredential input)
        {
            // определяем ключ
            setKey(input.UserName);

            // получаем размер следующего поля
            countSybol = input.Password.Length / key.Length;

            int nextField = key.Length * (countSybol + 1);

            countSybol = nextField - input.Password.Length;

            // дополняет пароль полями для ключа
            for (int i = 0; i < countSybol; i++)
                input.Password += input.Password[i];
                
            string result = "";

            // меняем в полях символы по ключу
            for (int i = 0; i < input.Password.Length; i += key.Length)
            {
                char[] transposition = new char[key.Length];

                for (int j = 0; j < key.Length; j++)
                {
                    char test = input.Password[i + j];
                    transposition[key[j] - 1] = test;
                }
                for (int j = 0; j < key.Length; j++)
                    result += transposition[j];
            }
            input.Password = result;
        }
        /// <summary>
        /// Создает ключ из имени учетной записи пользователя
        /// </summary>
        /// <param name="userName"></param>
        private static void setKey(string userName)
        {
            int keySize = userName.Length;

            userName = userName.ToLower();
            // сортируем по алфавиту
            string userNameSorted = string.Join("", userName.ToCharArray(0, userName.Length).OrderBy(x => x));

            List<int> ints = new List<int>();

            // делаем перестановку в соответствии с алфавитом
            for(int i = 0; i < keySize; i++)
            {
                for(int j = 1; j < keySize; j++)
                {
                    if(!ints.Contains(j))
                    {
                        if (userName[i] == userNameSorted[j])
                            ints.Add(j);
                    }
                }
            }

            key = ints.ToArray();
        }
    }
}
