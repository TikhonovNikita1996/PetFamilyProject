using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Application.DataBase;

namespace PetFamily.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dbContext;

    public UnitOfWork(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}