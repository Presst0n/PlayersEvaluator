using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using PE.WPF.EventModels;
using PE.WPF.Models;
using PE.WPF.UILibrary.Models;

namespace PE.WPF.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>, IHandle<RosterEvent>
    {
        private readonly IEventAggregator _events;
        private ILoggedInUserModel _loggedInUser;
        private RosterManagementViewModel _rosterManagementVM;

        public ShellViewModel(IEventAggregator events, ILoggedInUserModel loggedInUser, RosterManagementViewModel rosterManagementVM)
        {
            _events = events;
            _loggedInUser = loggedInUser;
            _rosterManagementVM = rosterManagementVM;

            _events.SubscribeOnPublishedThread(this);
            NotifyOfPropertyChange(() => CanLogOut);
            NotifyOfPropertyChange(() => CanLoadMyRosters);
            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }

        public bool CanLogOut
        {
            get
            {
                return !string.IsNullOrEmpty(_loggedInUser.Token);
            }
        }

        public bool CanLoadMyRosters
        {
            get
            {
                return !string.IsNullOrEmpty(_loggedInUser.Token);
            }
        }

        public async Task DisplayRegisterView()
        {
            await ActivateItemAsync(IoC.Get<RegisterViewModel>());
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(IoC.Get<MyRostersViewModel>());
            NotifyOfPropertyChange(() => CanLogOut);
            NotifyOfPropertyChange(() => CanLoadMyRosters);
        }

        public async Task HandleAsync(RosterEvent message, CancellationToken cancellationToken)
        {
            var t = (Roster)message.Data;
            t.Raiders.ForEach(raider => raider.Name = raider.Name.Replace(",", ""));

            _rosterManagementVM.Roster = t;
            await ActivateItemAsync(_rosterManagementVM);
        }

        public async Task LoadMyRosters()
        {
            await ActivateItemAsync(IoC.Get<MyRostersViewModel>());
            NotifyOfPropertyChange(() => CanLoadMyRosters);
        }

        public async Task LogOut()
        {
            _loggedInUser.LogOffUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => CanLogOut);
        }

        public async Task CloseApplication()
        {
            await TryCloseAsync();
        }
    }
}
