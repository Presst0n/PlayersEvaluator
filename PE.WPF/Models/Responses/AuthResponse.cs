using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace PE.WPF.Models.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public IEnumerable<string> CustomErrors { get; set; }
        public IEnumerable<Errors> Errors { get; set; }
    }
}
