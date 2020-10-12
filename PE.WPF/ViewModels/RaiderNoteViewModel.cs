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

        public RaiderNoteViewModel(ILoggedInUserModel loggedInUser, INoteService noteService)
        {
            _loggedInUser = loggedInUser;
            _noteService = noteService;
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

        public void AddNote()
        {
            RaiderNotes.Add(new RaiderNote
            {
                CreatorId = _loggedInUser.UserId,
                CreatorName = _loggedInUser.UserName,
                Message = NewNote,
                RaiderId = Raider.RaiderId,
            });

            Raider.Notes = RaiderNotes.ToList();

            var result = _noteService.CreateRaiderNote(Raider.RaiderId, NewNote);
            NewNote = "";
        }

        public void EditNote()
        {
            if (_loggedInUser.UserName != SelectedNote.CreatorName)
            {
                return;
            }

            SelectedNote.Message = _eNote;
            var result = _noteService.EditRaiderNote(SelectedNote.RaiderNoteId, SelectedNote.Message);
            SelectedNote = null;
            Raider.Notes = RaiderNotes.ToList();
            RaiderNotes = null;
            RaiderNotes = new ObservableCollection<RaiderNote>(Raider.Notes);
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            _selectedNote = null;
            RaiderNotes = new ObservableCollection<RaiderNote>(Raider.Notes);
            return base.OnActivateAsync(cancellationToken);
        }
    }
}
