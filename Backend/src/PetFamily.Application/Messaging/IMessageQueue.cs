namespace PetFamily.Application.Messaging;

public interface IMessageQueue<TMessage>
{
    public Task WriteAsync(TMessage fileInfos,
        CancellationToken cancellationToken = default);

    public Task<TMessage> ReadAsync(CancellationToken cancellationToken = default);
}