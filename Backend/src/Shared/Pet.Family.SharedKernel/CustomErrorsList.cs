using System.Collections;

namespace Pet.Family.SharedKernel;

public class CustomErrorsList : IEnumerable<CustomError>
{
    private readonly List<CustomError> _errors;

    public CustomErrorsList(IEnumerable<CustomError> errors)
    {
        _errors = [.. errors];
    }

    public IEnumerator<CustomError> GetEnumerator()
    {
        return _errors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static implicit operator CustomErrorsList(List<CustomError> errors) => new(errors);

    public static implicit operator CustomErrorsList(CustomError error) => new([error]);
}