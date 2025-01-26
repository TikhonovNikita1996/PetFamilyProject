using FileService.Infrastructure;
using FileService.Infrastructure.Repositories;
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
}