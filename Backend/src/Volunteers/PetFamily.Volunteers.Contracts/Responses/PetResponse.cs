using PetFamily.Core.Dtos.Pet;

namespace PetFamily.Volunteers.Contracts.Responses;

public class PetResponse
{
    public Guid Id { get; init; }
    public Guid VolunteerId { get; init; }
    public Guid SpecieId { get; init; }
    public Guid BreedId { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Age { get; init; }
    public string Description { get; init; } = string.Empty;
    public string CurrentStatus { get; init; } = string.Empty;
    public string Gender { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public string HealthInformation { get; init; } = string.Empty;
    public double Weight { get; init; }
    public double Height { get; init; }
    public bool IsSterilized { get; init; }
    public bool IsVaccinated { get; init; }
    public IReadOnlyList<Guid> PhotosId { get; init; } = [];
    public IReadOnlyList<string> PhotosUrls { get; init; } = [];

    public static PetResponse ToPetResponse(PetDto dto, IReadOnlyList<Guid> photosIds,
        IReadOnlyList<string> photosUrls) =>
        new PetResponse
        {
            Id = dto.Id,
            VolunteerId = dto.VolunteerId,
            SpecieId = dto.SpecieId,
            BreedId = dto.BreedId,
            Name = dto.Name,
            Age = dto.Age,
            Description = dto.Description,
            CurrentStatus = dto.CurrentStatus,
            Gender = dto.Gender,
            Color = dto.Color,
            HealthInformation = dto.HealthInformation,
            Weight = dto.Weight,
            Height = dto.Height,
            IsSterilized = dto.IsSterilized,
            IsVaccinated = dto.IsVaccinated,
            PhotosId = photosIds,
            PhotosUrls = photosUrls
        };
}

