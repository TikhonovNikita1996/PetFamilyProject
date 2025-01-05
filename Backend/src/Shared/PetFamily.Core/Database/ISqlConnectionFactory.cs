using System.Data;
using Npgsql;

namespace PetFamily.Core.Database;

public interface ISqlConnectionFactory
{
    public IDbConnection CreateConnection();
}