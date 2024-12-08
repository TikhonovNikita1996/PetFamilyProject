﻿using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Others;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Volunteer;

public class Volunteer : BaseEntity<VolunteerId>, ISoftDeletable
{
    private readonly List<Pet.Pet> _pets = [];
    private bool _isDeleted = false;
    
    // ef core
    public Volunteer(VolunteerId id) : base(id) {}

    public Volunteer(VolunteerId id, FullName fullname, Email email,
        GenderType gender, WorkingExperience workingExperience,
        Description description, PhoneNumber phoneNumber,
        DonationInfoList donationInfoList, SocialMediaDetails socialMediaDetails) : base(id)
    {
        Fullname = fullname;
        Email = email;
        Gender = gender;
        WorkingExperience = workingExperience;
        Description = description;
        PhoneNumber = phoneNumber;
        DonateForHelpInfos = donationInfoList;
        SocialMediaDetails = socialMediaDetails;
    }
    public FullName Fullname { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public GenderType Gender { get; private set; } = GenderType.Male;
    public WorkingExperience WorkingExperience { get; private set; } = default!;
    public Description Description { get; private set; } = default!;
    public PhoneNumber PhoneNumber { get; private set; } = default!;
    public DonationInfoList DonateForHelpInfos { get; private set; }
    public SocialMediaDetails SocialMediaDetails { get; private set; }
    public IReadOnlyList<Pet.Pet> CurrentPets => _pets;
    public int PetsWhoFoundHome => _pets.Count(p => p.CurrentStatus == HelpStatusType.FoundHome);
    public int PetsSearchingForHome => _pets.Count(p => p.CurrentStatus == HelpStatusType.SearchingForHome);
    public int PetsOnTreatment => _pets.Count(p => p.CurrentStatus == HelpStatusType.OnTreatment);
    
    public static Result<Volunteer, CustomError> Create(VolunteerId id,
                                                FullName fullname, Email email,
                                                GenderType gender, WorkingExperience workingExperience, Description description,
                                                PhoneNumber phoneNumber, DonationInfoList donationInfoList,
                                                SocialMediaDetails socialMediaDetails)
    {
        
        var volunteer = new Volunteer(id, fullname, email, gender, workingExperience, description,
            phoneNumber, donationInfoList, socialMediaDetails);

        return volunteer;
    }
    
    public void UpdateMainInfo(FullName fullName, 
        Description description, 
        PhoneNumber phoneNumber, 
        WorkingExperience workingExperience)
    {
        Fullname = fullName;
        Description = description;
        PhoneNumber = phoneNumber;
        WorkingExperience = workingExperience;
    }
    
    public void UpdateDonationInfo(DonationInfoList NewDonateForHelpInfos)
    {
        DonateForHelpInfos = NewDonateForHelpInfos;
    }
    public void UpdateSocialMediaDetails(SocialMediaDetails NewSocialMediaDetails)
    {
        SocialMediaDetails = NewSocialMediaDetails;
    }

    public void Delete()
    {
        if (!_isDeleted)
            _isDeleted = true;
    }
    
    public void Restore()
    {
        if (_isDeleted)
            _isDeleted = false;
    }

    public void AddPet(Pet.Pet pet)
    {
        _pets.Add(pet);
    }
    
    public Result<Pet.Pet, CustomError> GetPetById(Guid petId)
    {
        var pet = CurrentPets.FirstOrDefault(p => p.Id == petId);
        if (pet is null)
            return Errors.General.NotFound(petId);

        return pet;
    }
}