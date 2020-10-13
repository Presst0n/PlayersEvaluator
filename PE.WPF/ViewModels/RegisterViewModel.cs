﻿using Caliburn.Micro;
using PE.DataManager.Dto;
using PE.WPF.EventModels;
using PE.WPF.Service.Interfaces;
using PE.WPF.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PE.WPF.ViewModels
{
    public sealed class RegisterViewModel : Screen
    {
        private string _userName;
        private string _email;
        private string _password;
        private string _errorMessage;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IEventAggregator _events;

        public RegisterViewModel(IAuthService authService, IUserService userService, IEventAggregator events)
        {
            DisplayName = "Register";
            _authService = authService;
            _userService = userService;
            _events = events;
        }

        public bool IsErrorVisible
        {
            get
            {
                bool output = false;
                if (ErrorMessage?.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => IsErrorVisible);
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }

        public string EmailAddress
        {
            get { return _email; }
            set
            {
                _email = value;
                NotifyOfPropertyChange(() => EmailAddress);
                NotifyOfPropertyChange(() => CanRegister);
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => EmailAddress);
                NotifyOfPropertyChange(() => CanRegister);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanRegister);
            }
        }

        public bool CanRegister
        {
            get
            {
                bool output = false;

                if (EmailAddress?.Length > 0 && !string.IsNullOrWhiteSpace(EmailAddress) && Password?.Length > 0 && !string.IsNullOrWhiteSpace(Password))
                {
                    output = true;
                }

                return output;
            }
        }

        public async Task Register()
        {
            try
            {
                ErrorMessage = "";
                var result = await _authService.RegisterAsync(EmailAddress, UserName, Password);

                if (result != null)
                {
                    await _userService.GetLoggedInUserAsync(result.Token, result.RefreshToken);
                    await _userService.CreateLocalUserAsync();
                    await _events.PublishOnUIThreadAsync(new LogOnEvent());
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                await Task.Delay(new TimeSpan(0, 0, 15)).ContinueWith(o => ErrorMessage = "");
            }
        }
    }
}
