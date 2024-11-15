namespace PetFamily.Domain.Shared;

public static class Errors
{
    public static class General
    {
        public static CustomError ValueIsInvalid(string? name = null)
        {
            var label = name ?? "value";
            return CustomError.Validation("value.is.invalid", $"{label} is invalid");
        }
        
        public static CustomError NotFound(Guid? id = null)
        {
            var forid = id == null ? "" : $" for id: {id}";
            return CustomError.NotFound("record.found", $"record not found for id : {forid}");
        }
        
        public static CustomError ValueIsRequired(string? name = null)
        {
            var label = name == null ? "" : " " + name + " ";
            return CustomError.Validation("value.is.invalid", $"{label} is invalid");
        }
        
    }
}