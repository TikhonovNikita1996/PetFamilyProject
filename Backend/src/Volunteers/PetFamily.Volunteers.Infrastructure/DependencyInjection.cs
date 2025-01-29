using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Minio.AspNetCore;
using Pet.Family.SharedKernel;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.Core.Messaging;
using PetFamily.Core.Providers;
using PetFamily.Volunteers.Application.Database;
using PetFamily.Volunteers.Application.Interfaces;
using PetFamily.Volunteers.Infrastructure.BackgroundServices;
using PetFamily.Volunteers.Infrastructure.DataContexts;
using PetFamily.Volunteers.Infrastructure.Files;
using PetFamily.Volunteers.Infrastructure.MessageQueues;
using PetFamily.Volunteers.Infrastructure.Repositories;
using PetFamily.Volunteers.Infrastructure.Services;
using ServiceCollectionExtensions = Minio.ServiceCollectionExtensions;

namespace PetFamily.Volunteers.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContexts(configuration)
            .AddRepositories()
            .AddUnitOfWork()
            .AddSoftDelete();

        services.AddHostedService<FilesCleanerBackgroundService>();
        services.AddScoped<IFileCleanerService, FileCleanerService>();
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();

        services.AddSingleton<IMessageQueue<IEnumerable<FileMetaData>>
            ,InMemoryMessageQueue<IEnumerable<FileMetaData>>>();
        return services;
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(ProjectConstants.Context.VolunteerManagement);
        return services;
    }
    
    private static IServiceCollection AddDbContexts(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<VolunteersWriteDbContext>(_ => 
            new VolunteersWriteDbContext(configuration.GetConnectionString("Database")!));
        
        services.AddScoped<IReadDbContext, VolunteersReadDbContext>(_ => 
            new VolunteersReadDbContext(configuration.GetConnectionString("Database")!));

        return services;
    }
    
    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IVolunteerRepository, VolunteerRepository> ();
        return services;
    }
    
    private static IServiceCollection AddSoftDelete(
        this IServiceCollection services)
    {
        services.AddHostedService<CleanSoftDeletedEntitiesBackGroundService>();
        services.AddScoped<DeleteExpiredPetsService>();
        services.AddScoped<DeleteExpiredVolunteersService>();
        
        return services;
    }
    
    // private static IServiceCollection AddMinioCustom(this IServiceCollection services,
    //     IConfiguration configuration)
    // {
    //     services.Configure<MinioOptions>(configuration.GetSection(Options.MinioOptions.MINIO));
    //     services.AddScoped<IFileService, MinioService>();
    //     ServiceCollectionExtensions.AddMinio(services, options =>
    //     {
    //         var minioOptions = configuration.GetSection(Options.MinioOptions.MINIO).Get<Options.MinioOptions>()
    //                            ?? throw new ApplicationException("Missing minio configuration");
    //         
    //         options.WithEndpoint(minioOptions.Endpoint);
    //         options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
    //         options.WithSSL(minioOptions.WithSSL);
    //     });
    //     
    //     return services;
    // }
}