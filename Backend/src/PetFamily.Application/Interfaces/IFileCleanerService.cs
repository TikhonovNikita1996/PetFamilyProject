namespace PetFamily.Application.Interfaces;

public interface IFileCleanerService
{
    public Task Process(CancellationToken cancellationToken);
}