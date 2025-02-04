using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Pet;
using Pet.Family.SharedKernel.ValueObjects.Specie;
using Pet.Family.SharedKernel.ValueObjects.Volunteer;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.StartUploadPhotosToPet;
using PetFamily.Volunteers.Domain.Ids;
using PetFamily.Volunteers.Infrastructure.DataContexts;

namespace PetFamily.Volunteer.IntegrationTests.Volunteers;

public class AddPhotosToPetTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly IntegrationTestsWebFactory _factory;
    private readonly IServiceScope _scope;
    private readonly VolunteersWriteDbContext _volunteersWriteDbContext;
    private readonly ICommandHandler<Guid,AddPhotosToPetCommand> _sut;

    public AddPhotosToPetTests(IntegrationTestsWebFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _volunteersWriteDbContext = _scope.ServiceProvider.GetRequiredService<VolunteersWriteDbContext>();
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, AddPhotosToPetCommand>>();
    }
    
    [Fact]
    public async Task Add_Photo_To_Pet_Result_Should_Be_Success()
    {
        _factory.SetupSuccessFileServiceMock();
        
        // Arrange
        var volunteer = await SeedVolunteerWithPet();
        
        var command = CreateCommand(volunteer.Id, volunteer.CurrentPets.First().Id);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        
    }
    
    [Fact]
    public async Task Add_Photo_To_UnExisted_Pet_Result_Should_Be_Failure()
    {
        _factory.SetupSuccessFileServiceMock();
        
        // Arrange
        var volunteer = await SeedVolunteerWithPet();
        
        var command = CreateCommand(volunteer.Id, PetId.Create(Guid.NewGuid()));
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        
    }
    
    private async Task<PetFamily.Volunteers.Domain.Volunteer.Volunteer> SeedVolunteerWithPet()
    {
        var volunteer = PetFamily.Volunteers.Domain.Volunteer.Volunteer.Create(
            VolunteerId.NewId(),
            Description.Create("Test description").Value,
            PhoneNumber.Create("+7-777-777-77-77").Value).Value;

        var pet = PetFamily.Volunteers.Domain.Pet.Pet.Create( PetId.NewId(), PetsName.Create("Test").Value,
            Age.Create(2).Value,
            new SpecieDetails(SpecieId.NewId(), BreedId.NewId()),
            GenderType.Female,
            PetsDescription.Create("Test description").Value,
            Color.Create("Black").Value,
            HealthInformation.Create("Test").Value,
            LocationAddress.Create("Test", "Test", "Test", "Test", "Test", "Test").Value,
            5,5, OwnersPhoneNumber.Create("+7-777-77-77-77").Value,
            true, true, HelpStatusType.NeedHelp, new DonationInfoList(null),DateTime.Now).Value;
        
        volunteer.AddPet(pet);
        
        await _volunteersWriteDbContext.Volunteers.AddAsync(volunteer);
        await _volunteersWriteDbContext.SaveChangesAsync();
        return volunteer;
    }
    
    // private async Task<Guid> SeedSpecieBreed(string? specieName = null, string? breedName = null)
    // {
    //     var sName =  specieName == null ? "Bad Test" : specieName;
    //     var bName =  breedName == null ? "Bad Test" : breedName;
    //         
    //     var specie = Species.Domain.ValueObjects.Specie.Create(SpecieId.NewId(), sName,
    //         new List<Breed>
    //         {
    //             new Breed(BreedId.NewId(), bName),
    //         });
    //
    //     await _writeDbContext.Species.AddAsync(specie.Value);
    //     await _writeDbContext.SaveChangesAsync();
    //     return specie.Value.Id;
    // }

    private AddPhotosToPetCommand CreateCommand(Guid volunteerId, Guid petId)
    {
        var photoDtos = new List<CreateFileDto>
        {
            new CreateFileDto(Stream.Null, "Test1.jpg"),
            new CreateFileDto(Stream.Null, "Test2.jpg"),
            new CreateFileDto(Stream.Null, "Test3.jpg")
        };
        
        return new AddPhotosToPetCommand(volunteerId,petId,photoDtos);
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await _factory.ResetDatabaseAsync();
    }
}