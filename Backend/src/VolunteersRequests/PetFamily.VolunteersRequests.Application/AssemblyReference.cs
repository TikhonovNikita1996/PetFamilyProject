using System.Reflection;

namespace PetFamily.VolunteersRequests.Application;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}