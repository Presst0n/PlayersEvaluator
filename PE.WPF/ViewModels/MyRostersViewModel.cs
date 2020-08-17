using Caliburn.Micro;
using PE.WPF.Models;
using PE.WPF.UILibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PE.WPF.ViewModels
{
    public class MyRostersViewModel : Screen
    {
        private BindingList<RosterInfo> _rosters;
        private RosterInfo _selectedRoster;
        private ILoggedInUserModel _loggedInUser;

        public MyRostersViewModel(ILoggedInUserModel loggedInUser)
        {
            _loggedInUser = loggedInUser;
        }

        public ILoggedInUserModel LoggedInUser
        {
            get { return _loggedInUser; }
        }

        public string LoggedInUserName
        {
            get { return _loggedInUser.UserName + "!"; }
        }

        public BindingList<RosterInfo> Rosters
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

        public RosterInfo SelectedRoster
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

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            LoadRosters();
        }

        public async Task LoadRoster()
        {
            // Handle loading roster from api by injecting service here responsible for that.
        }


        public async Task DeleteRoster()
        {
            // Handle deleting selected roster from list
        }

        public void RefreshRosters()
        {
            LoadRosters();
        }

        private void LoadRosters()
        {
            var rosters = new List<RosterInfo>();

            for (int i = 0; i < 8; i++)
            {
                rosters.Add(new RosterInfo
                {
                    RosterId = i + 1,
                    CreationDate = DateTime.Now.ToShortTimeString(),
                    RosterName = $"Super gildia {i + 1}",
                    GuildLogo = "https://logo.placeholder",
                    Description = "Roster Core grupy gildii Res Publica. Najlepszej polskiej gildii na kazzaku",
                    Raiders = RandomizeRaiders()
                });
            }

            Rosters = new BindingList<RosterInfo>(rosters);
        }

        private List<string> RandomizeRaiders()
        {
            var raiders = new List<string>() { "Rupert", "Stach", "Gamer1234", "WaltDruid", "PresstonFury", "Destroyer1337", "Amanti", "RedDog3",
                "Egon", "TankSpec", "Lolooloxd", "MightyLock", "Trevor", "Ridget", "Elton", "Xantaras", "Wiggin", "Yasmin", "Elrond", "Ugway",
                "Jaxs", "Trvevolt", "Bolvar", "Beowulf", "Wiklet", "Geralt", "Fingolfin"
            };

            Random rand = new Random();

            var rngRaiders = new List<string>();

            for (int i = 0; i < 10; i++)
            {
                rngRaiders.Add(raiders.OrderBy(x => rand.NextDouble()).First());
            }

            return rngRaiders.Select(x => x + ',').ToList();
        }
    }
}
