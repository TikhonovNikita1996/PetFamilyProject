using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Minio.AspNetCore;
using PetFamily.Application.DataBase;
using PetFamily.Application.Interfaces;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Messaging;
using PetFamily.Core.Providers;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.Infrastructure.DataContexts;
using PetFamily.Infrastructure.Files;
using PetFamily.Infrastructure.Interceptors;
using PetFamily.Infrastructure.MessageQueues;
using PetFamily.Infrastructure.Options;
using PetFamily.Species.Application.Database;
using PetFamily.Species.Application.Interfaces;
using PetFamily.Species.Infrastructure;
using PetFamily.Species.Infrastructure.DataContexts;
using ServiceCollectionExtensions = Minio.ServiceCollectionExtensions;

namespace PetFamily.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContexts(configuration)
                .AddRepositories()
                .AddUnitOfWork()
                .AddMinioCustom(configuration);

        services.AddHostedService<FilesCleanerBackgroundService>();
        services.AddScoped<IFileCleanerService, FileCleanerService>();

        services.AddSingleton<IMessageQueue<IEnumerable<FileMetaData>>
            ,InMemoryMessageQueue<IEnumerable<FileMetaData>>>();
        return services;
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
    
    private static IServiceCollection AddDbContexts(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<WriteDbContext>(_ => 
            new WriteDbContext(configuration.GetConnectionString("Database")!));
        
        services.AddScoped<IReadDbContext, ReadDbContext>(_ => 
            new ReadDbContext(configuration.GetConnectionString("Database")!));

        return services;
    }
    
    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IVolunteerRepository, VolunteerRepository> ();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();

        return services;
    }
    
    private static IServiceCollection AddMinioCustom(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.MINIO));
        services.AddScoped<IFileService, MinioService>();
        ServiceCollectionExtensions.AddMinio(services, options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                               ?? throw new ApplicationException("Missing minio configuration");
            
            options.WithEndpoint(minioOptions.Endpoint);
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSSL);
        });
        
        return services;
    }
}