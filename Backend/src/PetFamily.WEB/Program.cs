using FileService.Communication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Infrastructure;
using PetFamily.Accounts.Presentation;
using PetFamily.API;
using PetFamily.API.Middlewares;
using PetFamily.Discussions.Application;
using PetFamily.Discussions.Presentation;
using PetFamily.Species.Application;
using PetFamily.Species.Presentation;
using PetFamily.Species.Presentation.Controllers;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Presentation;
using PetFamily.Volunteers.Presentation.Controllers;
using PetFamily.VolunteersRequests.Application;
using PetFamily.VolunteersRequests.Infrastructure;
using Serilog;
using Serilog.Events;

DotNetEnv.Env.Load();

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
    .AddSpeciesApplication()
    .AddAccountsInfrastructure(builder.Configuration)
    .AddAccountsApplication()
    .AddAuthorizationServices(builder.Configuration)
    .AddAccountsPresentation()
    .AddVolunteersRequestsInfrastructure(builder.Configuration)
    .AddDiscussionsInfrastructure(builder.Configuration)
    .AddDiscussionApplication()
    .AddVolunteersRequestsApplication()
    .AddFileHttpCommunication(builder.Configuration);

builder.Services.AddSerilog();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

var accountSeeder = app.Services.GetRequiredService<AccountsSeeder>();

// await accountSeeder.SeedAsync();

app.UseExceptionMiddleware();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(config =>
{
    config
        .WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

namespace PetFamily.API
{
    public partial class Program { }
}
