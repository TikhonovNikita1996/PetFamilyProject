using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.API.Contracts;

public record CreateSpecieRequest(string Name, List<BreedDto> Breeds);
