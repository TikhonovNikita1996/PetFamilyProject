using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.DataBase;

public interface IReadDbContext
{
    DbSet<VolunteerDto> Volunteers { get;}
}