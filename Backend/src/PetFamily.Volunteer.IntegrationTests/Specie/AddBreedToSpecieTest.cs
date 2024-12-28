using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pet.Family.SharedKernel.ValueObjects;
using Pet.Family.SharedKernel.ValueObjects.Specie;
using PetFamily.Core.Abstractions;
using PetFamily.Species.Application.Commands.AddBreed;
using PetFamily.Species.Infrastructure.DataContexts;

namespace PetFamily.Volunteer.IntegrationTests.Specie;

public class AddBreedToSpecieTest : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly IntegrationTestsWebFactory _factory;
    private readonly IServiceScope _scope;
    private readonly WriteDbContext _writeDbContext;
    private readonly ICommandHandler<Guid, AddBreedCommand> _sut;

    public AddBreedToSpecieTest(IntegrationTestsWebFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _writeDbContext = _scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, AddBreedCommand>>();
    }
    
    [Fact]
    public async Task Add_Breed_To_Specie_Result_Should_Be_Success()
    {
        // Arrange
        var specieId = await SeedSpecie();
        var command = new AddBreedCommand(specieId, "Breed");
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        
        var specieResult = await _writeDbContext.Species
            .FirstOrDefaultAsync();

        var breedsCount = specieResult.Breeds.Count; 
        
        breedsCount.Should().Be(1);
    }
    
    [Fact]
    public async Task Add_Breed_To_Unexisted_Specie_Result_Should_Be_Failure()
    {
        // Arrange
        var specieId = await SeedSpecie();
        var command = new AddBreedCommand(SpecieId.NewId(), "Breed");
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsFailure.Should().BeTrue();
    }

    private async Task<Guid> SeedSpecie()
    {
        var specie = Species.Domain.ValueObjects.Specie.Create(SpecieId.NewId(), "Test specie");

        await _writeDbContext.Species.AddAsync(specie.Value);
        await _writeDbContext.SaveChangesAsync();
        return specie.Value.Id;
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await _factory.ResetDatabaseAsync();
    }
}