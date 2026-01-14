using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTest.Application.Common.Interfaces;
using TechTest.Infrastructure.Persistence;
using TechTest.Infrastructure.Users;

namespace TechTest.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TechTestDbContext>(opt =>
                opt.UseNpgsql(configuration.GetConnectionString("Default")));

            services.AddScoped<DbConnectionFactory>();
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();
            services.AddScoped<IUserReadRepository, UserReadRepository>();

            return services;
        }
    }
}
