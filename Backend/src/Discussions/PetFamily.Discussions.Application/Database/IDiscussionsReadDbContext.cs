using PetFamily.Core.Dtos.Discussion;

namespace PetFamily.Discussions.Application.Database;

public interface IDiscussionsReadDbContext
{
    public IQueryable<DiscussionDto> Discussions { get; }
}