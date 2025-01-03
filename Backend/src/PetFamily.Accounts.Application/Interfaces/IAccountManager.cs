using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using PetFamily.Accounts.Domain.AccountModels;

namespace PetFamily.Accounts.Application.Interfaces;

public interface IAccountManager
{
    public Task<UnitResult<CustomErrorsList>> CreateAdminAccount(AdminAccount adminAccount);
    public Task<UnitResult<CustomErrorsList>> CreateParticipantAccount(ParticipantAccount adminAccount);
    public Task<UnitResult<CustomErrorsList>> CreateVolunteerAccount(VolunteerAccount adminAccount);
}