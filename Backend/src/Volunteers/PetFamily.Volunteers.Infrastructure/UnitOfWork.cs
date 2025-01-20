﻿using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Infrastructure.DataContexts;

namespace PetFamily.Volunteers.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly VolunteersWriteDbContext _dbContext;

    public UnitOfWork(VolunteersWriteDbContext dbContext)
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