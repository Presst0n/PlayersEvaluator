using System;
using System.Collections.Generic;
using System.Text;

namespace PE.Contracts.V1.Responses
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> CustomErrors { get; set; }
        public IEnumerable<ErrorModel> Errors { get; set; }
    }
}
