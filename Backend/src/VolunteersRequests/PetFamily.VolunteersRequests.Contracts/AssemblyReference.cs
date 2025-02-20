using System.Reflection;

namespace PetFamily.VolunteersRequests.Contracts;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}