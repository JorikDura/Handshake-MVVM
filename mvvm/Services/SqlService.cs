using Microsoft.Data.SqlClient;
using mvvm.Models;
using mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace mvvm.Services
{
    public class SqlService
    {
        private readonly SqlConnection _connection;
        private const string DB_PATH_CONNECTION = 
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\chymishhh\source\repos\mvvm\mvvm\DateBase\Users.mdf;Integrated Security=True";

        public SqlService()
        {
            _connection = new SqlConnection(DB_PATH_CONNECTION);
            try
            {
                _connection.Open();
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Проверка на существование аккаунта
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        public bool isAuthorized(NetworkCredential credential)
        {
            SqlCommand command = new SqlCommand(
                "SELECT 1 FROM Users WHERE Username = @username AND Password = @password", 
                _connection);
            command.Parameters.Add("@username", SqlDbType.NVarChar).Value = 
                credential.UserName;
            command.Parameters.Add("@password", SqlDbType.NVarChar).Value = 
                credential.Password;
            return Convert.ToBoolean(command.ExecuteScalar());
        }
        /// <summary>
        /// Возвразает информацию о пользователе
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        public UserModel getUserData(NetworkCredential credential)
        {
            UserModel userModel = new UserModel();
            SqlCommand command = new SqlCommand(
                "SELECT * FROM Users WHERE Username = @username AND Password = @password", 
                _connection);
            command.Parameters.Add("@username", SqlDbType.NVarChar).Value =
                credential.UserName;
            command.Parameters.Add("@password", SqlDbType.NVarChar).Value =
                credential.Password;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    userModel.Id = reader.GetInt32(0);
                    userModel.Username = reader.GetString(1);
                    userModel.Password = reader.GetString(2);
                    userModel.Name = reader.GetString(3);
                    userModel.LastName = reader.GetString(4);
                    userModel.Phone = reader.GetString(5);
                    userModel.isAdmin = reader.GetBoolean(6);
                    userModel.isBanned = reader.GetBoolean(7);
                }
            }
            return userModel;
        }
        /// <summary>
        /// Добавляет нового пользователя
        /// </summary>
        /// <param name="credential"></param>
        /// <param name="Name"></param>
        /// <param name="LastName"></param>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public bool insertNewUser(NetworkCredential credential, string Name, string LastName, string Phone)
        {
            SqlCommand command = new SqlCommand(
                "INSERT INTO Users (Username, Password, Name, LastName, Phone, isAdmin, isBanned) VALUES (@login, @password, @name, @lastname, @phone, 0, 0)", 
                _connection);
            command.Parameters.Add("@login", SqlDbType.NVarChar).Value =
                credential.UserName;
            command.Parameters.Add("@password", SqlDbType.NVarChar).Value =
                credential.Password;
            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = 
                Name;
            command.Parameters.Add("@lastname", SqlDbType.NVarChar).Value = 
                LastName;
            command.Parameters.Add("@phone", SqlDbType.NVarChar).Value = 
                Phone;
            return Convert.ToBoolean(command.ExecuteNonQuery());
        }
        /// <summary>
        /// Возвращает всех пользователей
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<UserModel> getAllUsers()
        {
            ObservableCollection<UserModel> userViewModels = new ObservableCollection<UserModel>();
            SqlCommand command = new SqlCommand(
                "SELECT Id, Username, Name, LastName, Phone, IsBanned FROM Users", 
                _connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    UserModel userModel = new UserModel();
                    userModel.Id = reader.GetInt32(0);
                    userModel.Username = reader.GetString(1);
                    userModel.Name = reader.GetString(2);
                    userModel.LastName = reader.GetString(3);
                    userModel.Phone = reader.GetString(4);
                    userModel.isAdmin = false;
                    userModel.isBanned = reader.GetBoolean(5);
                    if(!userModel.isBanned)
                        userViewModels.Add(userModel);
                }
            }
            return userViewModels;
        }
        /// <summary>
        /// Возвращает минимальный размер пароля
        /// </summary>
        /// <returns></returns>
        public int getPasswordSize()
        {
            SqlCommand command = new SqlCommand(
                "SELECT MinPasswordSize FROM Settings WHERE Id=1", 
                _connection);
            return Convert.ToInt32(command.ExecuteScalar());
        }
        /// <summary>
        /// Изменить размер минимального пароля
        /// </summary>
        /// <param name="passwordSize"></param>
        public void changePasswordSize(int passwordSize)
        {
            SqlCommand command = new SqlCommand(
                "UPDATE SETTINGS SET MinPasswordSize = @value WHERE Id=1", 
                _connection);
            command.Parameters.Add("@value", SqlDbType.Int).Value = passwordSize;
            command.ExecuteNonQuery();
        }
        /// <summary>
        /// Изменить пароль пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <param name="credential"></param>
        public void changePassword(int id, NetworkCredential credential)
        {
            SqlCommand command = new SqlCommand("UPDATE Users SET Password = @password WHERE Id = @id", _connection);
            command.Parameters.Add("@password", SqlDbType.NVarChar).Value = credential.Password;
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            command.ExecuteNonQuery();
        }
        /// <summary>
        /// Забанить пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool banUser(UserModel user)
        {
            SqlCommand command = new SqlCommand(
                "UPDATE Users SET isBanned = 1 WHERE Id = @id", 
                _connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = user.Id;
            return Convert.ToBoolean(command.ExecuteNonQuery());
        }
        /// <summary>
        /// Использование чисел и мат. символов
        /// </summary>
        /// <returns></returns>
        public bool getIsPresence()
        {
            SqlCommand command = new SqlCommand(
                "SELECT isPresenceNumbers FROM Settings WHERE Id=1", 
                _connection);
            return Convert.ToBoolean(command.ExecuteScalar());
        }

        public void changePresence(bool value)
        {
            SqlCommand command = new SqlCommand(
                "UPDATE Settings SET isPresenceNumbers=@value WHERE Id=1", 
                _connection);
            command.Parameters.Add("@value", SqlDbType.Bit).Value = value;
            command.ExecuteNonQuery();
        }
    }
}
