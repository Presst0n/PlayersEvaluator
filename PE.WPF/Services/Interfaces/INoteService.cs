using System.Threading.Tasks;

namespace PE.WPF.Services.Interfaces
{
    public interface INoteService
    {
        Task<bool> CreateRaiderNote(string raiderId, string message);
        Task<bool> EditRaiderNote(string raiderNoteId, string message);
    }
}