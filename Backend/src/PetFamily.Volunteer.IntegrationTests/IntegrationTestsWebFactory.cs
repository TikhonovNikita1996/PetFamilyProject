using System.Data.Common;
using Azure;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using NSubstitute;
using PetFamily.API;
using PetFamily.Application.DataBase;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.DataContexts;
using Respawn;
using Testcontainers.PostgreSql;

namespace PetFamily.Volunteer.IntegrationTests;

public class IntegrationTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IFileService _fileService = Substitute.For<IFileService>();
    
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("pet_family_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();
    
    private Respawner _respawner = default!;
    private DbConnection _dbConnection = default!;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefaultServices);
    }

    protected virtual void ConfigureDefaultServices(IServiceCollection services)
    {
        var writeContext = services.SingleOrDefault(s =>
            s.ServiceType == typeof(WriteDbContext));

        var readDbContext = services.SingleOrDefault(s => 
            s.ServiceType == typeof(IReadDbContext));
        
        if(writeContext != null)
            services.Remove(writeContext);
        
        if(readDbContext != null)
            services.Remove(readDbContext);
        
        services.AddScoped<WriteDbContext>(_ =>
            new WriteDbContext(_dbContainer.GetConnectionString()));
        
        services.AddScoped<IReadDbContext, ReadDbContext>(_ =>
            new ReadDbContext(_dbContainer.GetConnectionString()));

        services.AddTransient<IFileService>(_ => _fileService);
    }
    
    
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        
        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        await InitializeRespawner();
    }

    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, 
        new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"]
        });
    }
    
    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public void SetupSuccessFileServiceMock()
    {
        IReadOnlyList<FilePath> response = new List<FilePath>
        {
            FilePath.Create("Test1.jpg").Value,
            FilePath.Create("Test2.jpg").Value,
            FilePath.Create("Test3.jpg").Value
        };
        
        _fileService.UploadFilesAsync(Arg.Any<IEnumerable<FileData>>(), Arg.Any<CancellationToken>())
        .Returns(Result.Success<IReadOnlyList<FilePath>, CustomError>(response));
    }
    
    public void SetupFailureFileServiceMock()
    {
        _fileService.UploadFileAsync(Arg.Any<FileData>(), Arg.Any<CancellationToken>())
            .Returns(Result.Failure<string, CustomError>(CustomError.Failure("Failure", "Failed to upload file")));
    }
}