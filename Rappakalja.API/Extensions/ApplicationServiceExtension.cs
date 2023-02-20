using Microsoft.EntityFrameworkCore;
using Npgsql;
using Rappakalja.API.Data;
using Rappakalja.API.Data.Repositories;
using Rappakalja.API.Interfaces;

namespace Rappakalja.API.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddCors();
            services.AddSignalR();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}