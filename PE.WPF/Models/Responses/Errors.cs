using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PE.WPF.Models.Responses
{
    public class Errors
    {
        [JsonProperty("fieldName")]
        public string FieldName { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("Message")]
        public string Message { get; set; }
    }
}
