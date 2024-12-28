namespace PetFamily.Core.Dtos.Pet;

public record UpdatePetsMainInfoDto(
    string Name,
    Guid SpeciesId,
    Guid BreedId,
    DateOnly BirthDate,
    string Gender,
    string Description,
    LocationAddressDto LocationAddressDto,
    double Weight,
    double Height,
    bool IsSterilized,
    bool IsVaccinated,
    string OwnersPhoneNumber,
    string Color);