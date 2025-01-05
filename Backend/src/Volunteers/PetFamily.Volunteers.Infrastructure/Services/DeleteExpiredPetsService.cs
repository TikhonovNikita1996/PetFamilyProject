using Dapper;
using Pet.Family.SharedKernel;
using PetFamily.Core.Database;

namespace PetFamily.Volunteers.Infrastructure.Services;

public class DeleteExpiredPetsService (ISqlConnectionFactory connectionFactory)
{
    public async Task Process()
    {
        var sqlQuery = """
                       DELETE
                       FROM "PetFamily_Volunteers"."pets" p
                       WHERE deletion_date < now() - make_interval(days => @LifeTimeDays);
                       """;

        var param = new DynamicParameters();

        param.Add("LifeTimeDays", ProjectConstants.SOFT_DELETED_ENTITIES_LIFE_TIME_IN_DAYS);

        var connection = connectionFactory.CreateConnection();

        await connection.ExecuteAsync(sqlQuery, param);
    }
}