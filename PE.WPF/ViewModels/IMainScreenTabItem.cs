using Caliburn.Micro;
using PE.WPF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PE.WPF.ViewModels
{
    public interface IMainScreenTabItem : IScreen
    {
        public Roster Roster { get; set; }
    }
}
