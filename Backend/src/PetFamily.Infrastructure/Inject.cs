using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Application.DataBase;
using PetFamily.Application.Interfaces;
using PetFamily.Infrastructure.Interceptors;
using PetFamily.Infrastructure.Options;
using PetFamily.Infrastructure.Providers;
using PetFamily.Infrastructure.Repositories;

namespace PetFamily.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<DataContext>();
        services.AddSingleton<SoftDeleteInterceptor>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IVolunteerRepository, VolunteerRepository> ();
        services.AddScoped<ISpeciesRepository, SpeciesRepository> ();
        services.AddMinioCustom(configuration);
        services.AddScoped<IFileProvider, MinioProvider>();
        return services;
    }

    private static IServiceCollection AddMinioCustom(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.MINIO));
        
        services.AddMinio(options =>
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