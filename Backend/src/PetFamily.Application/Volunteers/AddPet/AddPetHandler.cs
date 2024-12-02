using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.DataBase;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.AddPet;

public class AddPetHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AddPetHandler(
        ILogger<AddPetHandler> logger,
        IVolunteerRepository volunteersRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _volunteerRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, CustomError>> Handle(AddPetCommand command,
        CancellationToken cancellationToken = default)
    {
        
    }
    
}