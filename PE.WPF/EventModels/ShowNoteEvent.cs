using PE.WPF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PE.WPF.EventModels
{
    public class ShowNoteEvent
    {
        public ShowNoteEvent(Raider raider)
        {
            Raider = raider;
        }

        public Raider Raider { get; set; }
    }
}
