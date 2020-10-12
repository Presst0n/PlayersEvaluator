using System;
using System.Collections.Generic;
using System.Text;

namespace PE.WPF.EventModels
{
    public class RosterEvent
    {
        public object Data { get; set; }

        public RosterEvent() {  }

        public RosterEvent(object data)
        {
            Data = data;
        }
    }
}
