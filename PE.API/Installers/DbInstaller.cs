using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PE.API.Data;
using PE.API.ExtendedModels;
using PE.API.Repository;
using PE.API.Services;
using PE.LoggerService;

namespace PE.API.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ExtendedIdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IRosterService, RosterService>();
            services.AddScoped<IRosterAccessService, RosterAccessService>();
            services.AddScoped<IRaiderService, RaiderService>();
            services.AddScoped<IRaiderNoteService, RaiderNoteService>();
            services.AddScoped<IResourceAuthorizationService, ResourceAuthorizationService>();
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
    }
}
