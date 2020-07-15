using Microsoft.EntityFrameworkCore;
using PE.DataManager.Dto;

namespace PE.DataManager
{
    public interface ISqLiteDbContext
    {
        DbSet<User> Users { get; set; }
    }
}