namespace PetFamily.Volunteers.Application.Interfaces;

public interface IFileCleanerService
{
    public Task Process(CancellationToken cancellationToken);
}