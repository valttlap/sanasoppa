// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Npgsql;
using Sanasoppa.API.Data;
using Sanasoppa.API.Extensions;
using Sanasoppa.API.Hubs;

Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddConfigurationServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

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

app.UseErrorHandler();
app.UseSecureHeaders();
app.UseCors();
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
    await context.Database.MigrateAsync().ConfigureAwait(false);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

app.Run();
