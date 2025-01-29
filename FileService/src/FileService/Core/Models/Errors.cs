namespace FileService.Core.Models;

public static class Errors
{
    public static class Files
    {
        public static CustomError FailUpload()
        {
            return CustomError.Failure("file.upload.failed", "Fail to upload file");
        }

        public static CustomError FailRemove()
        {
            return CustomError.Failure("file.remove.failed", "Fail to remove file");
        }
        
        public static CustomError NotFound()
        {
            return CustomError.NotFound("file.not.found", "File not found");
        }

    }
}