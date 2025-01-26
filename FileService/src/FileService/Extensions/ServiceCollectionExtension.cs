using FileService.Infrastructure;
using FileService.Infrastructure.Providers;
using FileService.Infrastructure.Repositories;
using FileService.Options;
using Minio;
using MongoDB.Driver;

namespace FileService.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IFilesRepository, FilesRepository>();
        
        return services;
    }
    
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMongoClient>(new MongoClient(configuration.GetConnectionString("MongoConnection")));
        services.AddScoped<FileMongoDbContext>();
        
        return services;
    }
    
    public static IServiceCollection AddMinioCustom(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection(Options.MinioOptions.MINIO));
        services.AddScoped<IFileProvider, MinioProvider>();
        ServiceCollectionExtensions.AddMinio(services, options =>
        {
            var minioOptions = configuration.GetSection(Options.MinioOptions.MINIO).Get<Options.MinioOptions>()
                               ?? throw new ApplicationException("Missing minio configuration");
            
            options.WithEndpoint(minioOptions.Endpoint);
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSSL);
        });
        
        return services;
    }
}