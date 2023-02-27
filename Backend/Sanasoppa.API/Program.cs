using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.Data;
using Sanasoppa.API.Entities;
using Sanasoppa.API.Extensions;
using Sanasoppa.API.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var connString = "";
if (builder.Environment.IsDevelopment())
    connString = builder.Configuration.GetConnectionString("DefaultConnection");
else
{
    var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

    if (connUrl == null)
    {
        throw new ArgumentNullException(nameof(connUrl), $"The value of '{nameof(connUrl)}' is null. Please set the environment variable 'DATABASE_URL' with a valid connection string.");
    }

    connUrl = connUrl.Replace("postgres://", string.Empty);
    var pgUserPass = connUrl.Split("@")[0];
    var pgHostPortDb = connUrl.Split("@")[1];
    var pgHostPort = pgHostPortDb.Split("/")[0];
    var pgDb = pgHostPortDb.Split("/")[1];
    var pgUser = pgUserPass.Split(":")[0];
    var pgPass = pgUserPass.Split(":")[1];
    var pgHost = pgHostPort.Split(":")[0];
    var pgPort = pgHostPort.Split(":")[1];

    connString = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};";
}
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseNpgsql(connString);
});

var app = builder.Build();
if (builder.Environment.IsDevelopment())
{
    app.UseCors(builder => builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins("http://localhost:4200"));
}

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.MapHub<GameHub>("hubs/gamehub");
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await Seed.SeedDefaultUser(userManager, roleManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

app.Run();