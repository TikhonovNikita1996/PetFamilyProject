﻿using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileManagement.Upload;

public class UploadFileHandler
{
    private readonly IFileService _fileService;

    public UploadFileHandler(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task<Result<string, CustomError>> Handle(
        UploadFileRequest request,
        CancellationToken cancellationToken = default)
    {
        var filePath = FilePath.Create(request.FilePath).Value;
        var fileData = new FileData(
            request.FileStream,
            new FileMetaData(request.BucketName, FilePath.Create(request.FilePath).Value));
        
        var result = await _fileService.UploadFileAsync(fileData, cancellationToken);

        return result;
    }
}