using System.Security.AccessControl;
using PetFamily.Domain.Enums;

namespace PetFamily.Domain;

public class Pet
{
    public Guid Id { get; private set; }
    public string PetsName { get; private set; } = default!;
    public string Species { get; private set; } = default!;
    public GenderType Gender { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string Breed { get; private set; } = default!;
    public string Color { get; private set; } = default!;
    public string HealthInformation { get; private set; } = default!;
    public string LocationAdress { get; private set; } = default!;
    public double Weight { get; private set; } = default!;
    public double Hight { get; private set; } = default!;
    public string OwnersPhoneNumber { get; private set;} = default!;
    public bool IsSterilized { get; private set; } = default!;
    public DateTime DateOfBirth { get; private set; } = default!;
    public bool IsVaccinated { get; private set; } = default!;
    public HelpStatusType CurrentStatus { get; private set; } = default!;
    public List<DetailsforAssistance> DetailsforAssistance { get; private set; } = [];
    public DateTime PetsPageCreationDate { get; private set; } = default!;
}