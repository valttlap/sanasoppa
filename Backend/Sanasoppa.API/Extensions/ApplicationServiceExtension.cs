using Microsoft.EntityFrameworkCore;
using Npgsql;
using Sanasoppa.API.Data;
using Sanasoppa.API.Data.Repositories;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddCors();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSignalR();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}