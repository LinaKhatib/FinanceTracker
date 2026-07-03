namespace FinanceTracker.Application.DTOs.Transactions;

public class GetTransactionsDto
{
    public Guid? CategoryId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? SortBy { get; set; } = "DateTime";
    public string? SortDirection { get; set; } = "desc"; 
}
