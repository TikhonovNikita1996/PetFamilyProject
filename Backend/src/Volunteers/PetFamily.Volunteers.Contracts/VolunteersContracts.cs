using CSharpFunctionalExtensions;

namespace PetFamily.Volunteers.Contracts;

public interface IVolunteerContracts
{
    Task<bool> IsAnyPetWithNeededSpecieExists(Guid speciesId,
        CancellationToken cancellationToken = default);
    
    Task<bool> IsAnyPetWithNeededBreedExists(Guid breedId,
        CancellationToken cancellationToken = default);
}