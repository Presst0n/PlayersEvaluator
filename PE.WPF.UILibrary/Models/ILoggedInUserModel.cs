using System;

namespace PE.WPF.UILibrary.Models
{
    public interface ILoggedInUserModel
    {
        DateTime CreationDate { get; set; }
        string Email { get; set; }
        string UserId { get; set; }
        string RefreshToken { get; set; }
        string Token { get; set; }
        string UserName { get; set; }
        DateTime LastLogIn { get; set; }

        void LogOffUser();
    }
}