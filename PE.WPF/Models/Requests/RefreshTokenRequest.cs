using System;
using System.Collections.Generic;
using System.Text;

namespace PE.WPF.Models.Requests
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
