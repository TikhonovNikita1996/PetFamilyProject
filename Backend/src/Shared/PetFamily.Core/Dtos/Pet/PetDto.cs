﻿namespace PetFamily.Core.Dtos.Pet;

public class PetDto
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
    public PhotoDto[] Photos { get; init; } = [];
}