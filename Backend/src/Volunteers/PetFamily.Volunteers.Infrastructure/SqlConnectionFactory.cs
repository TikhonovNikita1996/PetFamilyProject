using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PetFamily.Core.Database;

namespace PetFamily.Volunteers.Infrastructure;

public class SqlConnectionFactory (IConfiguration configuration) : ISqlConnectionFactory
{
    public IDbConnection CreateConnection() =>
        new NpgsqlConnection(configuration.GetConnectionString("Database"));
}