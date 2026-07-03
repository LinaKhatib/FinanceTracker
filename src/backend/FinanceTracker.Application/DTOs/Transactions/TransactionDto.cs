namespace FinanceTracker.Application.DTOs.Transactions;

public class TransactionDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public Guid CategoryId { get; set; }
    public required string CategoryName { get; set; } 
}