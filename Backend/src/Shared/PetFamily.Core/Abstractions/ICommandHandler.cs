using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;

namespace PetFamily.Core.Abstractions;

public interface ICommandHandler<TResponse, in TCommand>
{
    public Task<Result<TResponse, CustomErrorsList>> Handle(TCommand command,
        CancellationToken cancellationToken);
}

public interface ICommandHandler< in TCommand> where TCommand : ICommand
{
    public Task<UnitResult<CustomErrorsList>> Handle(TCommand command,
        CancellationToken cancellationToken);
}