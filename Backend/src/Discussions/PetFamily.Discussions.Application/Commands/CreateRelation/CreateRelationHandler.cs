using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Discussions.Application.Repositories;
using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Application.Commands.CreateRelation;

public class CreateRelationHandler : ICommandHandler<Relation, CreateRelationCommand>
{
    private readonly IRelationRepository _relationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRelationHandler(IRelationRepository relationRepository,
        [FromKeyedServices(ProjectConstants.Context.Discussions)] IUnitOfWork unitOfWork)
    {
        _relationRepository = relationRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Relation, CustomErrorsList>> Handle(CreateRelationCommand command,
        CancellationToken cancellationToken)
    {
        var existedRelation = await _relationRepository.GetByRelatedEntityId(command.RelationEntityId,
            cancellationToken);
        if (existedRelation.IsSuccess)
            return Errors.General.AlreadyExists("relation").ToErrorList();
        
        var relation = Relation.Create(command.RelationEntityId);

        var result = await _relationRepository.Add(relation, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        return result;
    }
}