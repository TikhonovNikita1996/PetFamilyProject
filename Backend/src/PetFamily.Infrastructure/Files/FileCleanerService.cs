using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Interfaces;
using PetFamily.Application.Messaging;

namespace PetFamily.Infrastructure.Files;

public class FileCleanerService : IFileCleanerService
{
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<FileCleanerService> _logger;
    private readonly IMessageQueue<IEnumerable<FileMetaData>> _messageQueue;

    public FileCleanerService(IFileProvider fileProvider,
        ILogger<FileCleanerService> logger, 
        IMessageQueue<IEnumerable<FileMetaData>> messageQueue)
    {
        _fileProvider = fileProvider;
        _logger = logger;
        _messageQueue = messageQueue;
    }

    public async Task Process(CancellationToken cancellationToken)
    {
        var fileInfos = await _messageQueue.ReadAsync(cancellationToken);

        foreach (var fileInfo in fileInfos)
        {
            await _fileProvider.DeleteFileAsync(fileInfo, cancellationToken);
        }
    }
}