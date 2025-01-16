namespace Pet.Family.SharedKernel;

public class ProjectConstants
{
    public const int MAX_LOW_TEXT_LENGTH = 100;
    public const int MAX_HIGHT_TEXT_LENGTH = 255;
    public const int MAX_HIGHT_PHONENUMBER_LENGTH = 20;
    public const int SOFT_DELETED_ENTITIES_LIFE_TIME_IN_HOURS = 24;
    public const int SOFT_DELETED_ENTITIES_LIFE_TIME_IN_DAYS = 30;
    public enum Context
    {
        VolunteerManagement,
        SpeciesManagement,
        AccountManagement,
        Discussions,
        VolunteersRequest
    }
}