using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Others;
using PetFamily.Domain.Entities.Pet;
using PetFamily.Domain.Entities.Pet.ValueObjects;
using PetFamily.Domain.Entities.Volunteer;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;
using Description = PetFamily.Domain.Entities.Volunteer.ValueObjects.Description;

namespace VolunteerTests;

public class VolunteerTests
{
    public Volunteer CreateTestVolunteer()
    {
        var volunteerId = VolunteerId.NewId();
        var fullName = FullName.Create("Test", "Test", "Test").Value;
        var email = Email.Create("aaaa@bbb.com").Value;
        var gender = GenderType.Male;
        var workingExp = WorkingExperience.Create(2).Value;
        var desctiption = Description.Create("Test").Value;
        var phoneNumber = PhoneNumber.Create("+7-777-777-77-77").Value;
        List<DonationInfo> donationInfos = new List<DonationInfo>()
        {
            DonationInfo .Create("Test1", "Test").Value,
            DonationInfo.Create("Test2", "Test").Value,
        };
        var donationInfoList = new DonationInfoList(donationInfos);
        
        List<SocialMedia> socialMedias = new List<SocialMedia>()
        {
            SocialMedia.Create("Test1", "asdasdasdasdasdasd").Value,
            SocialMedia.Create("Test2", "asdasdasdasdasdasd").Value,
        };

        var socialMediaList = new SocialMediaDetails(socialMedias);
        
        return Volunteer.Create(volunteerId, fullName, email,
            gender, workingExp, desctiption,
            phoneNumber, donationInfoList, socialMediaList).Value;
    }

    public Pet CreateTestPet()
    {
        var petId = PetId.NewId();
        var name = PetsName.Create("Bob").Value;
        var specieId = SpecieId.NewId();
        var breedId = BreedId.NewId();
        var specieDetails = SpecieDetails.Create(specieId, breedId).Value;
        var gender = GenderType.Male;
        var desctiption = PetsDescription.Create("Test").Value;
        var color = Color.Create("Red").Value;
        var healthInformation = HealthInformation.Create("Test").Value;
        var address = LocationAddress.Create("Test", "Test", "Test", "Test", "Test","Test").Value;
        double weight = 10;
        double height = 10;
        var phoneNumber = OwnersPhoneNumber.Create("+7-777-777-77-77").Value;
        bool isSterilized = true;
        DateTime birthday = DateTime.Now;
        bool isVaccinated = true;
        HelpStatusType helpStatus = HelpStatusType.SearchingForHome;
        List<DonationInfo> donationInfos = new List<DonationInfo>()
        {
            DonationInfo .Create("Test1", "Test").Value,
            DonationInfo.Create("Test2", "Test").Value,
        };
        var donationInfoList = new DonationInfoList(donationInfos);
        DateTime createdAt = DateTime.Now;

        return Pet.Create(petId, name, specieDetails, gender, desctiption, color,
            healthInformation, address, weight, height,
            phoneNumber, isSterilized, birthday, isVaccinated, 
            helpStatus, donationInfoList, createdAt).Value;
    }
    
    [Fact]
    public void Add_New_Pet_To_Pet_List()
    {
        //Arrange
        var volunteer = CreateTestVolunteer();
        var pet = CreateTestPet();
        var expectedValue = 1;

        //Act
        volunteer.AddPet(pet);

        //Assert
        Assert.Equal(expectedValue, volunteer.CurrentPets.Count);
        Assert.Equal(expectedValue, pet.PositionNumber.Value);
    }
    
    [Fact]
    public void MovePet_ItShould_Not_Move_PetsPosition_Up()
    {
        //Arrange
        var volunteer = CreateTestVolunteer();
        var pet = CreateTestPet();

        //Act
        volunteer.AddPet(pet);
        pet.MoveUp(volunteer.CurrentPets.Count);

        //Assert
        Assert.Equal(1, pet.PositionNumber.Value);
    }
    
    [Fact]
    public void MovePet_ItShould_Move_PetsPosition_To_Any_Position_And_Update_The_Rest_of_Pets_Positions()
    {
        //Arrange
        var volunteer = CreateTestVolunteer();
        var pet1 = CreateTestPet();
        var pet2 = CreateTestPet();
        var pet3 = CreateTestPet();
        var pet4 = CreateTestPet();

        //Act
        volunteer.AddPet(pet1);
        volunteer.AddPet(pet2);
        volunteer.AddPet(pet3);
        volunteer.AddPet(pet4);
        volunteer.ChangePetsPosition(pet1, 3);
        
        //Assert
        Assert.Equal(3, pet1.PositionNumber.Value);
        Assert.Equal(1, pet2.PositionNumber.Value);
        Assert.Equal(2, pet3.PositionNumber.Value);
        Assert.Equal(4, pet4.PositionNumber.Value);
    }
    
    [Fact]
    public void MovePet_ItShould_Not_Move_PetsPosition_To_Current_Position()
    {
        //Arrange
        var volunteer = CreateTestVolunteer();
        var pet1 = CreateTestPet();
        var pet2 = CreateTestPet();
        var pet3 = CreateTestPet();
        var pet4 = CreateTestPet();

        //Act
        volunteer.AddPet(pet1);
        volunteer.AddPet(pet2);
        volunteer.AddPet(pet3);
        volunteer.AddPet(pet4);
        volunteer.ChangePetsPosition(pet1, 2);
        
        //Assert
        Assert.Equal(2, pet1.PositionNumber.Value);
        Assert.Equal(1, pet2.PositionNumber.Value);
        Assert.Equal(3, pet3.PositionNumber.Value);
        Assert.Equal(4, pet4.PositionNumber.Value);
    }
    
    [Fact]
    public void MovePet_ItShould_Not_Move_PetsPosition_To_Out_Of_Range_Position()
    {
        //Arrange
        var volunteer = CreateTestVolunteer();
        var pet1 = CreateTestPet();
        var pet2 = CreateTestPet();
        var pet3 = CreateTestPet();
        var pet4 = CreateTestPet();

        //Act
        volunteer.AddPet(pet1);
        volunteer.AddPet(pet2);
        volunteer.AddPet(pet3);
        volunteer.AddPet(pet4);
        volunteer.ChangePetsPosition(pet1, 7);
        
        //Assert
        Assert.Equal(1, pet1.PositionNumber.Value);
        Assert.Equal(2, pet2.PositionNumber.Value);
        Assert.Equal(3, pet3.PositionNumber.Value);
        Assert.Equal(4, pet4.PositionNumber.Value);
    }
    
}