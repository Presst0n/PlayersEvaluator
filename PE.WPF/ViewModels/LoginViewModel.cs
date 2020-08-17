using Caliburn.Micro;
using System;
using System.Threading.Tasks;
using PE.WPF.Services;
using PE.WPF.EventModels;

namespace PE.WPF.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName;
        private string _password;
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

        public async Task LogIn()
        {
            try
            {
                ErrorMessage = "";
                var result = await _authService.AuthenticateAsync(UserName, Password);

                if (result != null)
                {
                    await _userService.GetLoggedInUserInfo(result.Token, result.RefreshToken);
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

                                                                                            //░░▄███▄███▄
                                                                                            //░░█████████
                                                                                            //░░▒▀█████▀░
                                                                                            //░░▒░░▀█▀
                                                                                            //░░▒░░█░
                                                                                            //░░▒░█
                                                                                            //░░░█
                                                                                            //░░█░░░░███████
                                                                                            //░██░░░██▓▓███▓██▒
                                                                                            //██░░░█▓▓▓▓▓▓▓█▓████
                                                                                            //██░░██▓▓▓(◐)▓█▓█▓█
                                                                                            //███▓▓▓█▓▓▓▓▓█▓█▓▓▓▓█
                                                                                            //▀██▓▓█░██▓▓▓▓██▓▓▓▓▓█
                                                                                            //░▀██▀░░█▓▓▓▓▓▓▓▓▓▓▓▓▓█
                                                                                            //░░░░▒░░░█▓▓▓▓▓█▓▓▓▓▓▓█
                                                                                            //░░░░▒░░░█▓▓▓▓█▓█▓▓▓▓▓█
                                                                                            //░▒░░▒░░░█▓▓▓█▓▓▓█▓▓▓▓█
                                                                                            //░▒░░▒░░░█▓▓▓█░░░█▓▓▓█
                                                                                            //░▒░░▒░░██▓██░░░██▓▓██
                                                                                            //████████████████████████
                                                                                            //█▄─▄███─▄▄─█▄─█─▄█▄─▄▄─█
                                                                                            //██─██▀█─██─██─█─███─▄█▀█
                                                                                            //▀▄▄▄▄▄▀▄▄▄▄▀▀▄▄▄▀▀▄▄▄▄▄▀