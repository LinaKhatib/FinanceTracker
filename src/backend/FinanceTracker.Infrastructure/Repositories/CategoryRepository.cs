using FinanceTracker.Application.Abstractions.Repositories;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

public class CategoryRepository(TransactionsDbContext context) : IRepository<Category>
{
    public async Task CreateAsync(Category category, CancellationToken cancellationToken = default)
    {
        await context.Categories.AddAsync(category, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Category category, CancellationToken cancellationToken = default)
    {
        context.Categories.Update(category);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Category category, CancellationToken cancellationToken = default)
    {
        context.Categories.Remove(category);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Set<Category>()
            .ToListAsync(cancellationToken);
    }
}