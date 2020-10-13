using Caliburn.Micro;
using PE.WPF.Models;
using PE.WPF.Services.Interfaces;
using PE.WPF.UILibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PE.WPF.ViewModels
{
    public class RaiderNoteViewModel : Screen
    {
        private RaiderNote _selectedNote;
        private string _newNote;
        private ILoggedInUserModel _loggedInUser;
        private string _eNote;
        private ObservableCollection<RaiderNote> _raiderNotes;
        private INoteService _noteService;
        private IRosterAccessService _accessService;

        public RaiderNoteViewModel(ILoggedInUserModel loggedInUser, INoteService noteService, IRosterAccessService accessService)
        {
            _loggedInUser = loggedInUser;
            _noteService = noteService;
            _accessService = accessService;
        }

        public Raider Raider { get; set; }

        public ObservableCollection<RaiderNote> RaiderNotes
        {
            get
            {
                return _raiderNotes;
            }
            set
            {
                _raiderNotes = value;
                NotifyOfPropertyChange(() => RaiderNotes);
            }
        }

        public RaiderNote SelectedNote
        {
            get
            {
                //if (_selectedNote != null)
                //{
                //    //if (!Raider.Notes.Exists(x => x.RaiderNoteId == _selectedNote.RaiderNoteId))
                //    //{
                //    //    return null;
                //    //}
                //}

                return _selectedNote;
            }
            set
            {
                _selectedNote = value;
                NotifyOfPropertyChange(() => SelectedNote);
                NotifyOfPropertyChange(() => CanShowEditNoteTextBox);
                NotifyOfPropertyChange(() => ENote);
                NotifyOfPropertyChange(() => CanDeleteNote);
            }
        }

        public bool CanShowEditNoteTextBox
        {
            get
            {
                bool output = false;

                if (SelectedNote != null && _loggedInUser.UserName == SelectedNote.CreatorName)
                {
                    output = true;
                }

                return output;
            }
        }

        public string NewNote
        {
            get
            {
                return _newNote;
            }
            set
            {
                _newNote = value;
                NotifyOfPropertyChange(() => NewNote);
                NotifyOfPropertyChange(() => CanAddNote);
            }
        }

        public bool CanAddNote
        {
            get
            {
                bool output = false;

                if (!string.IsNullOrEmpty(NewNote))
                {
                    output = true;
                }

                return output;
            }
        }

        public bool CanDeleteNote
        {
            get
            {
                bool output = false;

                if (SelectedNote != null)
                {
                    output = true;
                }

                return output;
            }
        }

        public string ENote
        {
            get
            {
                //if (SelectedNote != null && !string.IsNullOrEmpty(_eNote))
                //{
                //    return SelectedNote.Message;
                //}

                return SelectedNote?.Message;
            }
            set
            {
                _eNote = value;
            }
        }

        public void ShowEditNoteTextBox()
        {

        }

        public async void AddNote()
        {
            RaiderNotes.Add(new RaiderNote
            {
                CreatorId = _loggedInUser.UserId,
                CreatorName = _loggedInUser.UserName,
                Message = NewNote,
                RaiderId = Raider.RaiderId,
            });

            Raider.Notes = RaiderNotes.ToList();

            var result = await _noteService.CreateRaiderNoteAsync(Raider.RaiderId, NewNote);
            NewNote = "";
        }

        public async void EditNote()
        {
            if (_loggedInUser.UserName != SelectedNote.CreatorName)
            {
                return;
            }

            SelectedNote.Message = _eNote;
            var result = await _noteService.EditRaiderNoteAsync(SelectedNote.RaiderNoteId, SelectedNote.Message);
            SelectedNote = null;
            Raider.Notes = RaiderNotes.ToList();
            RaiderNotes = null;
            RaiderNotes = new ObservableCollection<RaiderNote>(Raider.Notes);
        }

        public async void DeleteNote()
        {
            var output = await _accessService.GetRosterAccessAsync(Raider.RosterId);
            var u = output.Data.ToList().SingleOrDefault(x => x.UserId == _loggedInUser.UserId);

            if (u is null )
            {
                // Display modal window with info, that user has no permissions to delete this resource.
                return;
            }

            if (u.IsOwner || u.IsModerator)
            {
                await _noteService.DeleteRaiderNoteAsync(SelectedNote.RaiderNoteId);

                RaiderNotes.Remove(SelectedNote);
                Raider.Notes = RaiderNotes.ToList();
            }
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            _selectedNote = null;
            RaiderNotes = new ObservableCollection<RaiderNote>(Raider.Notes);
            return base.OnActivateAsync(cancellationToken);
        }
    }
}
