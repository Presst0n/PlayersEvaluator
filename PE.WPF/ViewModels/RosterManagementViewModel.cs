using Caliburn.Micro;
using PE.WPF.EventModels;
using PE.WPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PE.WPF.ViewModels
{
    public class RosterManagementViewModel : Conductor<object>.Collection.OneActive, IHandle<ShowNoteEvent>
    {
        private readonly IEventAggregator _events;
        private List<IMainScreenTabItem> _tabs;
        private RegisterViewModel _registerVM;
        private RaiderNoteViewModel _raiderNoteVM;
        private IWindowManager _windowManager;

        public RosterManagementViewModel(IEventAggregator events, IEnumerable<IMainScreenTabItem> tabs, RegisterViewModel raiderNoteVM,
            RaiderNoteViewModel testVM, IWindowManager windowManager)
        {
            _events = events;
            _tabs = tabs.ToList();
            _events.SubscribeOnPublishedThread(this);
            _registerVM = raiderNoteVM;
            _raiderNoteVM = testVM;
            _windowManager = windowManager;
        }

        public Roster Roster { get; set; }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            PassRosterToTabItems();
            Items.AddRange(_tabs);
            return base.OnActivateAsync(cancellationToken);
        }

        public async Task HandleAsync(ShowNoteEvent message, CancellationToken cancellationToken)
        {
            _raiderNoteVM.Raider = message.Raider;
            await _windowManager.ShowDialogAsync(_raiderNoteVM);
        }

        private void PassRosterToTabItems()
        {
            _tabs.ForEach(t => t.Roster = Roster);
        }
    }
}
