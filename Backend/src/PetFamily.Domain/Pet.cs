using System.Security.AccessControl;

namespace PetFamily.Domain;

public class Pet
{
    public Guid Id { get; set; }
    public required string PetsName { get; set; }
    public required string Species { get; set; }
    public required string Description { get; set; }
    public required string Breed { get; set; }
    public required string Color { get; set; }
    public required string HealthInformation { get; set; }
    public required string LocationAdress { get; set; }
    public required double Weight { get; set; }
    public required double Hight { get; set; }
    public required string OwnersPhoneNumber { get; set; }
    public required bool IsSterilized { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public required bool IsVaccinated { get; set; }
    public required string CurrentStatus { get; set; }
    public required string DetailsforAssistance { get; set; }
    public required DateTime PetsPageCreationDate { get; set; }
}