using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.Update.DonationInfo;
using PetFamily.Application.Volunteers.Update.MainInfo;
using PetFamily.Application.Volunteers.Update.SocialMediaDetails;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<UpdateSocialMediaDetailsHandler>();
        services.AddScoped<UpdateDonationInfoHandler>();
        services.AddScoped<DeleteVolunteerHandler>();
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        return services;
    }
}