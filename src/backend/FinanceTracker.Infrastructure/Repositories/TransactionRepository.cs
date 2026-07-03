using FinanceTracker.Application.Abstractions.Repositories;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

public class TransactionRepository (TransactionsDbContext context): IRepository<Transaction>
    
{
    public async Task CreateAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        await context.Transactions.AddAsync(transaction, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        context.Transactions.Update(transaction);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        context.Transactions.Remove(transaction);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Transactions.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Set<Transaction>()
            .OrderBy(t => t.Date)
            .Include(t => t.Category)
            .ToListAsync(cancellationToken);
    }
}