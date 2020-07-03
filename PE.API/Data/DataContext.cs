using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PE.API.Dtos;
using PE.API.ExtendedModels;
using System.Linq;

namespace PE.API.Data
{
    public class DataContext : IdentityDbContext<ExtendedIdentityUser>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) 
        { 
        }

        public DbSet<RosterDto> RostersDto { get; set; }
        public DbSet<RaiderDto> RaidersDto { get; set; }
        public DbSet<RaiderNoteDto> RaiderNotesDto { get; set; }
        public DbSet<RefreshTokenDto> RefreshTokensDto { get; set; }
        public DbSet<UserRosterAccessDto> UserRosterAccessesDto { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var foreignKey in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Cascade;
            }


            //builder.Entity<RaiderDto>().Ignore(z => z.RaidRoster);
            //builder.Entity<RaiderNoteDto>().Ignore(z => z.Raider);
        }
    }
}
