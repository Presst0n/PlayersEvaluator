using System;
using System.Collections.Generic;
using System.Text;

namespace PE.Contracts.V1.Requests
{
    public class CreateRaiderNoteRequest
    {
        public string RaiderId { get; set; }
        public string Message { get; set; }
    }
}
