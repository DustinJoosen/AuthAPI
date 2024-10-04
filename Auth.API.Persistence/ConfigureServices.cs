using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Persistence
{
    public static class ConfigureServices
    {
        public static IServiceCollection ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Database"));
                options.EnableDetailedErrors();
            });

            return services;
        }
    }
}
