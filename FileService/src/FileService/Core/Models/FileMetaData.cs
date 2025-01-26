using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FileService.Core.Models;

public class FileMetadata
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }

    public string Key { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    
    public string BucketName { get; set; } = string.Empty;
    
    public string ContentType { get; set; } = string.Empty;
    
    public required DateTime UploadDate { get; init; }
}