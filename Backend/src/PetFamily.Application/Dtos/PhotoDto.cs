namespace PetFamily.Application.Dtos;

public class PhotoDto
{
    public string PathToStorage { get; set; } = string.Empty;
    public bool IsMain { get; set; }
}