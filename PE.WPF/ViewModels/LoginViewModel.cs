using Caliburn.Micro;
using System;
using System.Threading.Tasks;
using PE.WPF.Services;
using PE.WPF.EventModels;
using PE.WPF.Service.Interfaces;

namespace PE.WPF.ViewModels
{
    public sealed class LoginViewModel : Screen
    {
        private string _emailAddress = "user@example.com";
        private string _password = "T3st0we24.";
        private string _errorMessage;
        private readonly IAuthService _authService;
        private readonly IEventAggregator _events;
        private readonly IUserService _userService;

        public LoginViewModel(IAuthService authService, IEventAggregator events, IUserService userService)
        {
            _authService = authService;
            _events = events;
            _userService = userService;
        }

        public override string DisplayName 
        {
            get 
            {
                return "LogIn";
            }
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
            get { return _emailAddress; }
            set
            {
                _emailAddress = value;
                NotifyOfPropertyChange(() => EmailAddress);
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

                if (EmailAddress?.Length > 0 && !string.IsNullOrWhiteSpace(EmailAddress) && Password?.Length > 0 && !string.IsNullOrWhiteSpace(Password))
                {
                    output = true;
                }

                return output;
            }
        }

        public async Task LogIn()
        {
            try
            {
                ErrorMessage = "";

                if (!await TryLogInByUsingRefreshToken())
                {
                    var authResult = await _authService.AuthenticateAsync(EmailAddress, Password);

                    if (!string.IsNullOrEmpty(authResult.Token) && !string.IsNullOrEmpty(authResult.RefreshToken))
                    {
                        await _userService.GetLoggedInUserAsync(authResult.Token, authResult.RefreshToken);
                        await _events.PublishOnUIThreadAsync(new LogOnEvent());
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                await Task.Delay(new TimeSpan(0, 0, 15))
                    .ContinueWith(o => ErrorMessage = "");
            }
        }

        public async Task<bool> TryLogInByUsingRefreshToken()
        {
            var user = await _userService.GetLocalUserByEmailAsync(EmailAddress);

            if (user != null)
            {
                var t = user.LastLoginDate.AddMinutes(5);

                if (DateTime.Now > t)
                {
                    var authResult = await _authService.RefreshUserLoginAsync(user.Token, user.RefreshToken);

                    if (!string.IsNullOrEmpty(authResult.Token) && !string.IsNullOrEmpty(authResult.RefreshToken))
                    {
                        await _userService.GetLoggedInUserAsync(authResult.Token, authResult.RefreshToken);
                        await _events.PublishOnUIThreadAsync(new LogOnEvent());

                        return true;
                    }
                }
            }

            return false;
        }
    }
}

//                                                                                                    ░░▄███▄███▄
//                                                                                                    ░░█████████
//                                                                                                    ░░▒▀█████▀░
//                                                                                                    ░░▒░░▀█▀
//                                                                                                    ░░▒░░█░
//                                                                                                    ░░▒░█
//                                                                                                    ░░░█
//                                                                                                    ░░█░░░░███████
//                                                                                                    ░██░░░██▓▓███▓██▒
//                                                                                                    ██░░░█▓▓▓▓▓▓▓█▓████
//                                                                                                    ██░░██▓▓▓(◐)▓█▓█▓█
//                                                                                                    ███▓▓▓█▓▓▓▓▓█▓█▓▓▓▓█
//                                                                                                    ▀██▓▓█░██▓▓▓▓██▓▓▓▓▓█
//                                                                                                    ░▀██▀░░█▓▓▓▓▓▓▓▓▓▓▓▓▓█
//                                                                                                    ░░░░▒░░░█▓▓▓▓▓█▓▓▓▓▓▓█
//                                                                                                    ░░░░▒░░░█▓▓▓▓█▓█▓▓▓▓▓█
//                                                                                                    ░▒░░▒░░░█▓▓▓█▓▓▓█▓▓▓▓█
//                                                                                                    ░▒░░▒░░░█▓▓▓█░░░█▓▓▓█
//                                                                                                    ░▒░░▒░░██▓██░░░██▓▓██
//                                                                                                    ████████████████████████
//                                                                                                    █▄─▄███─▄▄─█▄─█─▄█▄─▄▄─█
//                                                                                                    ██─██▀█─██─██─█─███─▄█▀█
//                                                                                                    ▀▄▄▄▄▄▀▄▄▄▄▀▀▄▄▄▀▀▄▄▄▄▄▀