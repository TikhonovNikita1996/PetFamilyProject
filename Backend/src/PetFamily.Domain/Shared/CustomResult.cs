namespace PetFamily.Domain.Shared;

public class CustomResult
{
    public CustomResult(bool isSuccess, string? error)
    {
        if(isSuccess && error is not null)
            throw new InvalidOperationException();
        
        if(isSuccess == false && error == null)
            throw new InvalidOperationException();
        
        IsSuccess = isSuccess;
        Error = error;
    }

    public string? Error { get; set; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    
    public static CustomResult Success() => new (true, null);
    
    public static implicit operator CustomResult(string error) => new (false, error);
    
}
public class CustomResult<T> : CustomResult
{
    private readonly  T _value;

    public CustomResult(T value, bool isSuccess, string? error) 
        : base(isSuccess, error)
    {
        _value = value;
    }

    public T Value => IsSuccess ? _value : 
        throw new InvalidOperationException("The value of the custom result is unaccessible.");
    
    public static CustomResult<T> Success<T> (T value) => new (value, true, null);
    public static CustomResult<T> Failure<T> (string error) => new (default!, false, error);
    
    public static implicit operator CustomResult<T>(T value) => new (value, true, null);
    public static implicit operator CustomResult<T>(string error) => new (default!, false, error);
}