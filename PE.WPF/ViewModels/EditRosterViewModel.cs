using Caliburn.Micro;
using PE.WPF.Models;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace PE.WPF.ViewModels
{
    public sealed class EditRosterViewModel : Screen, IMainScreenTabItem
    {
        private string _rosterName;
        private BindingList<Raider> _raiders;
        private Raider _selectedRaider;

        public EditRosterViewModel()
        {
            DisplayName = "Edit Roster";
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            RosterName = Roster.Name;
            Raiders = new BindingList<Raider>(Roster.Raiders);
            return base.OnActivateAsync(cancellationToken);
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

        public BindingList<Raider> Raiders
        {
            get 
            { 
                return _raiders; 
            }
            set
            {
                _raiders = value;
                NotifyOfPropertyChange(() => Raiders);
            }
        }

        public string RosterName
        {
            get
            {
                return _rosterName;
            }
            set
            {
                _rosterName = value;
                NotifyOfPropertyChange(() => RosterName);
                NotifyOfPropertyChange(() => CanSaveChanges);
            }
        }

        public bool CanSaveChanges
        {
            get
            {
                bool output = false;

                if (RosterName?.Length > 0 && !string.IsNullOrWhiteSpace(RosterName))
                {
                    output = true;
                }

                return output;
            }
        }

        public void SaveChanges()
        {
            Roster.Name = RosterName;
        }
    }
}
