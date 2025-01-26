using FileService.Core;
using FileService.Core.Models;
using MongoDB.Driver;

namespace FileService.Infrastructure;

public class FileMongoDbContext(IMongoClient mongoClient)
{
    private readonly IMongoDatabase _database = mongoClient.GetDatabase("Files_Db");

    public IMongoCollection<FileData> Files => _database.GetCollection<FileData>("files");
}   