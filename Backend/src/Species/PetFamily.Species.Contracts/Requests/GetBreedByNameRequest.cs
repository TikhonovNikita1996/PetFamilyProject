namespace PetFamily.Species.Contracts.Requests;

public record GetBreedByNameRequest(Guid SpecieId, string BreedName);