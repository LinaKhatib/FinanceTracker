using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Transaction
{
    
    public Guid Id { get; init; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = new Category { Name = "Другое", Type = TransactionType.Income};
}