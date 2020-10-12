using Caliburn.Micro;
using PE.WPF.EventModels;
using PE.WPF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PE.WPF.ViewModels
{
    public sealed class RosterViewModel : Screen, IMainScreenTabItem
    {
        private Raider _selectedRaider;
        private readonly IEventAggregator _events;

        public RosterViewModel(IEventAggregator events)
        {
            DisplayName = "Roster";
            _events = events;
        }

        public Roster Roster { get; set; }

        public Raider SelectedRaider
        {
            get 
            { 
                return _selectedRaider; 
            }
            set
            {
                _selectedRaider = value;
                NotifyOfPropertyChange(() => SelectedRaider);
            }
        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
        }

        public async Task DisplayRaiderNotes()
        {
            await _events.PublishOnUIThreadAsync(new ShowNoteEvent(SelectedRaider));
        }
    }
}
