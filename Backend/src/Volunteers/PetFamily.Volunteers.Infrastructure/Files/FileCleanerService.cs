using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Messaging;
using PetFamily.Core.Providers;
using PetFamily.Volunteers.Application.Interfaces;

namespace PetFamily.Volunteers.Infrastructure.Files;

public class FileCleanerService : IFileCleanerService
{
    private readonly IFileService _fileService;
    private readonly ILogger<FileCleanerService> _logger;
    private readonly IMessageQueue<IEnumerable<FileMetaData>> _messageQueue;

    public FileCleanerService(IFileService fileService,
        ILogger<FileCleanerService> logger, 
        IMessageQueue<IEnumerable<FileMetaData>> messageQueue)
    {
        _fileService = fileService;
        _logger = logger;
        _messageQueue = messageQueue;
    }

    public async Task Process(CancellationToken cancellationToken)
    {
        var fileInfos = await _messageQueue.ReadAsync(cancellationToken);

        foreach (var fileInfo in fileInfos)
        {
            await _fileService.DeleteFileAsync(fileInfo, cancellationToken);
        }
    }
}