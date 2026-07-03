using System.Reflection;
using System.Transactions;
using FinanceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Transaction = FinanceTracker.Domain.Entities.Transaction;

namespace FinanceTracker.Infrastructure.DbContexts;

public class TransactionsDbContext : DbContext
{
    public TransactionsDbContext(DbContextOptions<TransactionsDbContext> options) : base(options)
    {
    }
    
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}