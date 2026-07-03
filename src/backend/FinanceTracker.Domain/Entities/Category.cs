using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Category
{
    
    public Guid Id { get; init; }
    public string Name { get; set; } = "Новая категория";
    public TransactionType Type { get; init; } 
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}