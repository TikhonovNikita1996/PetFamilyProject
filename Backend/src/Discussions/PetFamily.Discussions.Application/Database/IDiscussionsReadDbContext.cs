using PetFamily.Core.Dtos.Discussion;

namespace PetFamily.Discussions.Application.Database;

public interface IDiscussionsReadDbContext
{
    public IQueryable<RelationDto> Relations { get; }
    public IQueryable<DiscussionDto> Discussions { get; }
}