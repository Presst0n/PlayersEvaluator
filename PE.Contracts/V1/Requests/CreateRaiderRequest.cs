using System;
using System.Collections.Generic;
using System.Text;

namespace PE.Contracts.V1.Requests
{
    public class CreateRaiderRequest
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public string MainSpecialization { get; set; }
        public string OffSpecialization { get; set; }
        public string Role { get; set; }
        public string RosterId { get; set; }
    }
}
