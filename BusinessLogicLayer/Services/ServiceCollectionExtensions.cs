using DataLayer.Context;
using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomIdentityAndDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("OnlineOfficeBoy")));

            services.AddScoped<IUnitOFWork, UnitOFWork>();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Order/AccessDenied";
            });

            services.AddIdentity<Employee, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;
            }).AddEntityFrameworkStores<ApplicationDbContext>();
        }
    }
}