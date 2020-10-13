using System.Threading.Tasks;

namespace PE.WPF.Services.Interfaces
{
    public interface INoteService
    {
        Task<bool> CreateRaiderNoteAsync(string raiderId, string message);
        Task<bool> DeleteRaiderNoteAsync(string raiderNoteId);
        Task<bool> EditRaiderNoteAsync(string raiderNoteId, string message);
    }
}