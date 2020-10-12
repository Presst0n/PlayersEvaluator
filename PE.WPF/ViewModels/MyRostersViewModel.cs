using Caliburn.Micro;
using PE.WPF.EventModels;
using PE.WPF.Models;
using PE.WPF.Service.Interfaces;
using PE.WPF.Services;
using PE.WPF.UILibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PE.WPF.ViewModels
{
    public class MyRostersViewModel : Screen
    {
        private BindingList<Roster> _rosters;
        private Roster _selectedRoster;
        private ILoggedInUserModel _loggedInUser;
        private readonly IRosterService _rosterService;
        private IAuthService _authService;
        private IUserService _userService;
        private IEventAggregator _events;

        public MyRostersViewModel(ILoggedInUserModel loggedInUser, IRosterService rosterService, IAuthService authService, 
            IUserService userService, IEventAggregator events)
        {
            _loggedInUser = loggedInUser;
            _rosterService = rosterService;
            _authService = authService;
            _userService = userService;
            _events = events;
        }

        public ILoggedInUserModel LoggedInUser
        {
            get { return _loggedInUser; }
        }

        public string LoggedInUserName
        {
            get 
            { 
                return _loggedInUser.UserName + "!"; 
            }
        }

        public BindingList<Roster> Rosters
        {
            get
            {
                return _rosters;
            }
            set
            {
                _rosters = value;
                NotifyOfPropertyChange(() => Rosters);
            }
        }

        public bool IsRosterPreviewVisible
        {
            get
            {
                bool output = false;
                if (SelectedRoster != null)
                {
                    output = true;
                }

                return output;
            }
        }

        public Roster SelectedRoster
        {
            get
            {
                return _selectedRoster;
            }
            set
            {
                _selectedRoster = value;
                NotifyOfPropertyChange(() => SelectedRoster);
                NotifyOfPropertyChange(() => IsRosterPreviewVisible);
                NotifyOfPropertyChange(() => CanLoadRoster);
                NotifyOfPropertyChange(() => CanDeleteRoster);
            }
        }

        public bool CanLoadRoster
        {
            get
            {
                bool output = false;
                if (SelectedRoster != null)
                {
                    output = true;
                }

                return output;
            }
        }

        public bool CanDeleteRoster
        {
            get
            {
                bool output = false;
                if (SelectedRoster != null)
                {
                    output = true;
                }

                return output;
            }
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await LoadRosters();
            await base.OnActivateAsync(cancellationToken);
        }

        public async Task LoadRoster()
        {
            // Get selected roster and open RosterManagement windows with this roster.

            await _events.PublishOnUIThreadAsync(new RosterEvent(SelectedRoster));
        }

        public async Task DeleteRoster()
        {
            // Handle deleting selected roster from list
        }

        public async Task RefreshRosters()
        {
            await LoadRosters();
        }

        private async Task LoadRosters()
        {
            try
            {
                var t = _loggedInUser.LastLogIn.AddMinutes(5);

                if (DateTime.Now >= t)
                {
                    var result = await _authService.RefreshUserLoginAsync(_loggedInUser.Token, _loggedInUser.RefreshToken);
                    await _userService.GetLoggedInUserAsync(result.Token, result.RefreshToken);
                }

                var rosters = await _rosterService.GetRostersAsync();

                rosters.Data.ToList().ForEach(x => x.Raiders.ForEach(y => y.Name += ','));
                Rosters = new BindingList<Roster>(rosters.Data.ToList());
            }
            catch (Exception ex)
            {
                if (ex.Message == "Unauthorized")
                {
                    await _events.PublishOnUIThreadAsync(new LogOnEvent());
                }

                // Log exceptions somewhere.
            }
        }
    }
}
