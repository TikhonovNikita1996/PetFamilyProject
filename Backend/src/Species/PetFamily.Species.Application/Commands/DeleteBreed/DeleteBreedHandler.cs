﻿using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Species.Application.Commands.Create;
using PetFamily.Species.Application.Database;
using PetFamily.Species.Application.Interfaces;

namespace PetFamily.Species.Application.Commands.DeleteBreed;

public class DeleteBreedHandler : ICommandHandler<Guid,DeleteBreedCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<CreateSpecieHandler> _logger;
    private readonly IValidator<DeleteBreedCommand> _validator;
    private readonly IReadDbContext _readDbContext;

    public DeleteBreedHandler(
        [FromKeyedServices(ProjectConstants.Context.SpeciesManagement)] IUnitOfWork unitOfWork,
        ISpeciesRepository speciesRepository,
        ILogger<CreateSpecieHandler> logger,
        IValidator<DeleteBreedCommand> validator,
        IReadDbContext readDbContext)
    {
        _unitOfWork = unitOfWork;
        _speciesRepository = speciesRepository;
        _logger = logger;
        _validator = validator;
        _readDbContext = readDbContext;
    }
    
    public async Task<Result<Guid, CustomErrorsList>> Handle(
        DeleteBreedCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        // var petsQuery = _readDbContext.Pets.AsQueryable();
        //
        // var firstPetWithBreed = await petsQuery
        //     .SingleOrDefaultAsync(p => p.SpecieId == command.SpecieId 
        //                                && p.BreedId == command.BreedId, cancellationToken);
        //
        // if (firstPetWithBreed != null)
        //     return Errors.General.DeleteFailure().ToErrorList();
        
        var specieResult = await _speciesRepository.GetById(command.SpecieId,cancellationToken);
        var specie = specieResult.Value;

        specie.DeleteBreed(command.BreedId);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Breed with id {breedId} was deleted from specie: {specieId} .",
            command.BreedId, specie.Id);

        return command.BreedId;
    }
}