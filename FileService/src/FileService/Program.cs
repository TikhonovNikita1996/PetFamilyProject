using Amazon.S3;
using CSharpFunctionalExtensions;
using FileService.Endpoints;
using FileService.Extensions;
using FileService.Infrastructure;
using FileService.Infrastructure.Repositories;
using FileService.Middlewares;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.FileProviders;
using MongoDB.Driver;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<FileMongoDbContext>();
var mongoClient = new MongoClient(builder.Configuration.GetConnectionString("MongoConnection"));

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq") ??
                 throw new ArgumentNullException("Seq"))
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

builder.Services.AddHangfire(configuration =>
    configuration.UsePostgreSqlStorage(c => 
        c.UseNpgsqlConnection(builder.Configuration.GetConnectionString("HangFireConnection"))));

builder.Services.AddHangfireServer();

builder.Services.AddSerilog();
builder.Services.AddRepositories();
builder.Services.AddMongoDb(builder.Configuration);
builder.Services.AddScoped<FileMongoDbContext>();
builder.Services.AddMinioCustom(builder.Configuration);

builder.Services.AddSingleton<IAmazonS3>(_ =>
{
    var config = new AmazonS3Config
    {
        ServiceURL = "http://localhost:9000",
        ForcePathStyle = true,
        UseHttp = true
    };

    return new AmazonS3Client("minio_admin", "minio_password", config);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddEndpoints();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionMiddleware();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();

app.MapEndpoints();

app.Run();