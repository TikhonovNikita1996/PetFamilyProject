using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteersRequests.Application.Commands.TakeInReview;

public record TakeInReviewCommand(Guid AdminId, Guid RequestId) : ICommand;