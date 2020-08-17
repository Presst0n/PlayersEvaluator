using System;

namespace PE.WPF.UILibrary.Models
{
    public class LoggedInUserModel : ILoggedInUserModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
