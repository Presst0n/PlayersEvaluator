using Caliburn.Micro;
using PE.DataManager.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace PE.WPF.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName;
        private string _password;

        public string UserName
        {
            get { return _userName; }
            set 
            { 
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public string Password
        {
            get { return _password; }
            set 
            { 
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn); 
            }
        }

        public bool CanLogIn
        {
            get
            {
                bool output = false;

                if (UserName?.Length > 0 && !string.IsNullOrWhiteSpace(UserName) && Password?.Length > 0 && !string.IsNullOrWhiteSpace(Password))
                {
                    output = true;
                }

                return output;
            }

        }

        public void LogIn()
        {
            Console.WriteLine($"UserName: {UserName},{Environment.NewLine}Password: {Password}");
        }
    }
}
