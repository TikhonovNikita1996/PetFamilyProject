// using FluentAssertions;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using Pet.Family.SharedKernel;
// using Pet.Family.SharedKernel.ValueObjects.Specie;
// using Pet.Family.SharedKernel.ValueObjects.Volunteer;
// using PetFamily.Core.Abstractions;
// using PetFamily.Core.Dtos;
// using PetFamily.Core.Dtos.Pet;
// using PetFamily.Core.Dtos.Volunteer;
// using PetFamily.Species.Domain.ValueObjects;
// using PetFamily.Species.Infrastructure.DataContexts;
// using PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.AddPet;
// using PetFamily.Volunteers.Domain.Ids;
// using PetFamily.Volunteers.Infrastructure.DataContexts;
//
// namespace PetFamily.Volunteer.IntegrationTests.Volunteers;
//
// public class AddVolunteerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
// {
//     private readonly IntegrationTestsWebFactory _factory;
//     private readonly IServiceScope _scope;
//     private readonly SpeciesWriteDbContext _speciesWriteDbContext;
//     private readonly ICommandHandler<Guid, AddPetCommand> _sut;
//
//     public AddVolunteerTests(IntegrationTestsWebFactory factory)
//     {
//         _factory = factory;
//         _scope = factory.Services.CreateScope();
//         _speciesWriteDbContext = _scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
//         _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, AddPetCommand>>();
//     }
//     
//     [Fact]
//     public async Task Add_Pet_To_Volunteer_Result_Should_Not_Be_Null()
//     {
//         // Arrange
//         var volunteerId = await SeedVolunteer();
//         //
//         // var speciebreed = SeedSpecieBreed("Test specie", "Test breed");
//         var command = CreateAddPetCommand(volunteerId, "Test specie", "Test breed");
//         
//         // Act
//         var result = await _sut.Handle(command, CancellationToken.None);
//         
//         // Assert
//         result.IsSuccess.Should().BeTrue();
//         result.Value.Should().NotBeEmpty();
//         
//         var volunteerResult = await _speciesWriteDbContext.Volunteers
//             .Include(v => v.CurrentPets)
//             .FirstOrDefaultAsync();
//
//         var pet= volunteerResult!.CurrentPets.FirstOrDefault(); 
//         
//         pet.Should().NotBeNull();
//     }
//
//     [Fact]
//     public async Task Add_Pet_To_Volunteer_With_Not_Existed_Specie_Result_Should_Be_Null()
//     {
//         // Arrange
//         var volunteerId = await SeedVolunteer();
//         // var speciebreed = SeedSpecieBreed();
//         var command = CreateAddPetCommand(volunteerId, "Test specie", "Test breed");
//         
//         // Act
//         var result = await _sut.Handle(command, CancellationToken.None);
//         
//         // Assert
//         result.IsFailure.Should().BeTrue();
//         
//         var volunteerResult = await _speciesWriteDbContext.Volunteers
//             .Include(v => v.CurrentPets)
//             .FirstOrDefaultAsync();
//
//         var pet= volunteerResult!.CurrentPets.FirstOrDefault(); 
//         
//         pet.Should().BeNull();
//     }
//     
//     private async Task<Guid> SeedVolunteer()
//     {
//         var volunteer = PetFamily.Volunteers.Domain.Volunteer.Volunteer.Create(
//             VolunteerId.NewId(),
//             Description.Create("Test description").Value,
//             PhoneNumber.Create("+7-777-777-77-77").Value).Value;
//
//         await _speciesWriteDbContext.Volunteers.AddAsync(volunteer);
//         await _speciesWriteDbContext.SaveChangesAsync();
//         return volunteer.Id.Value;
//     }
//     
//     // private async Task<Guid> SeedSpecieBreed(string? specieName = null, string? breedName = null)
//     // {
//     //     var sName =  specieName == null ? "Bad Test" : specieName;
//     //     var bName =  breedName == null ? "Bad Test" : breedName;
//     //         
//     //     var specie = Species.Domain.ValueObjects.Specie.Create(SpecieId.NewId(), sName,
//     //         new List<Breed>
//     //         {
//     //             new Breed(BreedId.NewId(), bName),
//     //         });
//     //
//     //     await _writeDbContext.Species.AddAsync(specie.Value);
//     //     await _writeDbContext.SaveChangesAsync();
//     //     return specie.Value.Id;
//     // }
//
//     private AddPetCommand CreateAddPetCommand(Guid volunteerId, string specieName, string breedName)
//     {
//         return new AddPetCommand(volunteerId,
//             new PetsNameDto("Test"),
//             2,
//             specieName, breedName,
//             "Male", new DescriptionDto("Test description"),
//             "Red", new HealthInformationDto("Test Health Information"),
//             new LocationAddressDto("t", "t", "t", "t", "t", "t"),
//             2,2, new PhoneNumberDto("+7-777-777-77-77"),
//             true, true, "SearchingForHome",
//             new List<DonationInfoDto>
//                 {new DonationInfoDto("Test", "Test"),},
//             DateTime.Now
//             );
//     }
//     
//     public Task InitializeAsync() => Task.CompletedTask;
//
//     public async Task DisposeAsync()
//     {
//         _scope.Dispose();
//         await _factory.ResetDatabaseAsync();
//     }
// }