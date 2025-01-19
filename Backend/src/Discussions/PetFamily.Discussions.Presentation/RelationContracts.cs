using System.Threading.Channels;
using PetFamily.Discussions.Application.Commands.Relations.CreateRelation;
using PetFamily.Discussions.Application.Database;
using PetFamily.Discussions.Contracts;

namespace PetFamily.Discussions.Presentation;

public class RelationContracts 
{
    // private readonly CreateRelationHandler _createRelationHandler;
    // private readonly IDiscussionsReadDbContext _discussionsReadDbContext;
    //
    // public RelationContracts(CreateRelationHandler createRelationHandler,
    //     IDiscussionsReadDbContext discussionsReadDbContext)
    // {
    //     _createRelationHandler = createRelationHandler;
    //     _discussionsReadDbContext = discussionsReadDbContext;
    // }
    // public async Task<bool> IsRelationExist(Guid relationId, CancellationToken cancellationToken = default)
    // {
    //     // var relationDto = _discussionsReadDbContext.Relations
    //     //     .FirstOrDefault(relation => relation.RelationId == relationId);
    //     //
    //     // if (relationDto is not null)
    //     //     return true;
    //     // else
    //         return false;
    // }

    // public async Task<Guid> CreateRelation(Guid id,
    //     CancellationToken cancellationToken = default)
    // {
    //     var command = new CreateRelationCommand(id);
    //     await _createRelationHandler.Handle(command, cancellationToken);
    //     return id;
    // }
}