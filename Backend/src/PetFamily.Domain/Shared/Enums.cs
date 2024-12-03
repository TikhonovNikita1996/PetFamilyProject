namespace PetFamily.Domain.Shared;
public enum HelpStatusType
{
    NeedHelp,
    SearchingForHome,
    FoundHome,
    OnTreatment
}

public enum GenderType
{
    Male,
    Female
}

public enum ErrorType
{
    Validation,
    NotFound,
    Failure,
    Conflict
}
