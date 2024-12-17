using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Abstractions;

public interface ICommandHandler<TResponse, in TCommand> where TCommand : ICommand
{
    public Task<Result<TResponse, CustomErrorsList>> Handle(TCommand command,
        CancellationToken cancellationToken);
}

public interface ICommandHandler< in TCommand> where TCommand : ICommand
{
    public Task<UnitResult<CustomErrorsList>> Handle(TCommand command,
        CancellationToken cancellationToken);
}