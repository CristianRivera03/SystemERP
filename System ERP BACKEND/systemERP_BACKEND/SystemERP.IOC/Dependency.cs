using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SystemERP.BLL.Services.Contract;
using SystemERP.BLL.Services.Implementation;
using SystemERP.DAL.DBContext;
using SystemERP.DAL.Repositories.Contract;
using SystemERP.DAL.Repositories.Implementation;
using SystemERP.Utility;

namespace SystemERP.IOC
{
    public static class Dependency
    {
        public static void DependecyInjections(this IServiceCollection services, IConfiguration configuration)
        {
            // DBContext
            services.AddDbContext<SystemErpDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("connectionDB"));
            });

            // Generic Repository
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // AutoMapper
            services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());

            // HttpContextAccessor
            services.AddHttpContextAccessor();

            // Utilities & Config
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddScoped<IJwtUtility, JwtUtility>();

            // Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICatalogService, CatalogService>();
            services.AddScoped<IActionLogService, ActionLogService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IWarehouseService, WarehouseService>();
            services.AddScoped<IInventoryService, InventoryService>();
        }
    }
}
