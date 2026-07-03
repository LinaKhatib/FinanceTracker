namespace FinanceTracker.Application.DTOs.Transactions;

public class UpdateTransactionDto
{
    public decimal? Amount { get; set; }
    public string? Description { get; set; }
    public DateTime? Date { get; set; }
    public Guid? CategoryId { get; set; }
}