using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Sanasoppa.API.Data;
using Sanasoppa.API.Entities;
using Sanasoppa.API.Exceptions;
using System.Text;

namespace Sanasoppa.API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config, IHostEnvironment env)
    {
        services.AddIdentityCore<AppUser>(opt =>
        {
            opt.Password.RequireLowercase = true;
            opt.Password.RequireUppercase = true;
            opt.Password.RequireDigit = true;
            opt.Password.RequireNonAlphanumeric = true;
            opt.Password.RequiredLength = 8;
            opt.User.RequireUniqueEmail = true;
            opt.SignIn.RequireConfirmedEmail = true;
            opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            opt.Tokens.PasswordResetTokenProvider = "PasswordResetTokenProvider";
        })
            .AddRoles<AppRole>()
            .AddDefaultTokenProviders()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddEntityFrameworkStores<DataContext>();

        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromHours(3);
        });

        services.Configure<DataProtectionTokenProviderOptions>("PasswordResetTokenProvider", options =>
        {
            options.TokenLifespan = TimeSpan.FromHours(1);
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding
                        .UTF8.GetBytes(config["TokenKey"] ?? throw new ConfigurationException("Token key not found"))),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                options.SaveToken = true;

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context => HandleMessageRecived(context)
                };
            });

        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
            opt.AddPolicy("RequireModeratorRole", policy => policy.RequireRole("Admin", "Moderator"));
            opt.AddPolicy("RequireMemberRole", policy => policy.RequireRole("Admin", "Moderator", "Member"));
        });

        return services;
    }

    private static Task HandleMessageRecived(MessageReceivedContext context)
    {

        var accessToken = context.Request.Query["access_token"];

        var path = context.HttpContext.Request.Path;
        if (!string.IsNullOrWhiteSpace(accessToken) && path.StartsWithSegments("/hubs"))
        {
            context.Token = accessToken;
        }

        return Task.CompletedTask;
    }


}
