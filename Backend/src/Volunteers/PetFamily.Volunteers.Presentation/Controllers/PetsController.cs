using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Dtos.Discussion;
using PetFamily.Core.Dtos.Pet;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetPetById;
using PetFamily.Volunteers.Application.VolunteersManagement.Queries.GetPetsWithFiltersAndPagination;
using PetFamily.Volunteers.Presentation.Requests.Pet;

namespace PetFamily.Volunteers.Presentation.Controllers;
[Authorize]
public class PetsController : BaseApiController
{
    [HttpGet]
    [Permission(Permissions.Pets.Read)]
    public async Task<ActionResult> GetAllPets(
        [FromServices] GetPetsWithPaginationAndFiltersHandler handler,
        [FromQuery] GetPetsWithPaginationAndFiltersRequest request, 
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        var result = await handler.Handle(query, cancellationToken);
        
        return Ok(result.Value); 
    }
    
    [Permission(Permissions.Pets.Read)]
    [HttpGet("/pet-by-id")]
    public async Task<ActionResult> GetPetById(
        [FromServices] GetPetByIdHandler handler,
        [FromQuery] GetPetByIdRequest request, 
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        var result = await handler.Handle(query, cancellationToken);
        
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value); 
    }
    
    [HttpGet("/test-pets")]
    public async Task<ActionResult> GetTestPets(
        CancellationToken cancellationToken)
    {
        List<RelationDto> users = GetPetsForTests(8);

        List<RelationDto> GetPetsForTests(int numberOfPets)
        {
            List<RelationDto> pets = new List<RelationDto>();

            for (int i = 0; i < numberOfPets; i++)
            {
                var pet = new RelationDto
                {
                    Id = Guid.NewGuid(),
                    Name = $"Pet {i + 1}",
                    Age = new Random().Next(0, 100),
                    Color = "Black",
                    CurrentStatus = "Need Help",
                    Description = "A pet",
                    Gender = "Male",
                    BreedId = Guid.NewGuid(),
                    HealthInformation = "Health Information",
                    Height = 1,
                    Weight = 1,
                    IsSterilized = true,
                    IsVaccinated = true
                };
                pets.Add(pet);
            }
            
            return pets;
        }

        return Ok(users);
    }
}