using PetFamily.Core.Abstractions;

namespace PetFamily.Discussions.Application.Queries.GetDiscussionById;

public record GetDiscussionByIdQuery(Guid DiscussionId) : IQuery;