using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos.Specie;
using PetFamily.Species.Application.Commands.Create;
using PetFamily.Species.Infrastructure.DataContexts;

namespace PetFamily.Volunteer.IntegrationTests.Specie;

public class CreateSpicieWithBreedsTest : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly IntegrationTestsWebFactory _factory;
    private readonly IServiceScope _scope;
    private readonly SpeciesWriteDbContext _writeDbContext;
    private readonly ICommandHandler<Guid, CreateSpecieCommand> _sut;

    public CreateSpicieWithBreedsTest(IntegrationTestsWebFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _writeDbContext = _scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateSpecieCommand>>();
    }
    
    [Fact]
    public async Task Create_Specie_Result_Should_Be_Success()
    {
        // Arrange
        var breeds = new List<CreateBreedDto>
        {
            new CreateBreedDto("Test breed 1"),
            new CreateBreedDto("Test breed 2"),
            new CreateBreedDto("Test breed 3"),
            
        };
        var command = new CreateSpecieCommand("Test specie", breeds);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        
        var specieResult = await _writeDbContext.Species
            .FirstOrDefaultAsync();

        var breedsCount = specieResult.Breeds.Count; 
        
        specieResult.Should().NotBeNull();
        breedsCount.Should().Be(3);
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await _factory.ResetDatabaseAsync();
    }
}