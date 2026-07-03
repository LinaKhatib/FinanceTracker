namespace FinanceTracker.Application.DTOs.Transactions;

public class CreateTransactionDto
{
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
}