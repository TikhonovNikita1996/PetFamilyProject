﻿using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Core.Abstractions;
using PetFamily.VolunteersRequests.Infrastructure.DataContexts;

namespace PetFamily.VolunteersRequests.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly VolunteersRequestWriteDbContext _dbContext;

    public UnitOfWork(VolunteersRequestWriteDbContext dbContext)
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