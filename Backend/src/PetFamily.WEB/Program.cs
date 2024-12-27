using PetFamily.API.Middlewares;
using PetFamily.Species.Application;
using PetFamily.Species.Infrastructure;
using PetFamily.Species.Presentation;
using PetFamily.Species.Presentation.Controllers;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Presentation;
using PetFamily.Volunteers.Presentation.Controllers;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq") ??
                 throw new ArgumentNullException("Seq"))
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

builder.Services.AddControllers()
    .AddApplicationPart(typeof(SpeciesController).Assembly)
    .AddApplicationPart(typeof(VolunteersController).Assembly)
    .AddApplicationPart(typeof(PetsController).Assembly);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddVolunteersInfrastructure(builder.Configuration)
    .AddSpeciesInfrastructure(builder.Configuration)
    .AddVolunteersApplication()
    .AddSpeciesApplication();

builder.Services.AddSerilog();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

app.UseExceptionMiddleware();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace PetFamily.API
{
    public partial class Program { }
}