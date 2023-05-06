using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Sanasoppa.API.Data;
using Sanasoppa.API.Entities;
using Sanasoppa.API.Extensions;
using Sanasoppa.API.Hubs;
using Sanasoppa.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddConfigurationServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration, builder.Environment);

var connString = "";
var connStringBuilder = new NpgsqlConnectionStringBuilder(
        builder.Configuration.GetConnectionString("DefaultConnection"));
if (builder.Environment.IsDevelopment())
{
    connStringBuilder.Password = builder.Configuration["DbPassword"];
}
connString = connStringBuilder.ConnectionString;

builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseNpgsql(connString);
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (builder.Environment.IsDevelopment())
{
    app.UseCors(builder => builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins("https://localhost:4200"));
}

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.MapHub<GameHub>("hubs/gamehub");
app.MapHub<LobbyHub>("hubs/lobbyhub");
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    var environment = services.GetRequiredService<IWebHostEnvironment>();
    await context.Database.MigrateAsync();
    await Seed.SeedDefaultUserAsync(userManager, roleManager, environment.IsDevelopment());
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

app.Run();
