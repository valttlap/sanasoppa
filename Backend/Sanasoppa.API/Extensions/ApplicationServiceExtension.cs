using Sanasoppa.API.Data.Repositories;
using Sanasoppa.API.Interfaces;
using Sanasoppa.API.Services;

namespace Sanasoppa.API.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddCors();
        services.AddScoped<ITokenService, TokenService>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddSignalR();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}