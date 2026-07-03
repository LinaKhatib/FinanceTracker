using FinanceTracker.Application.DTOs.Transactions;

namespace FinanceTracker.Application.Abstractions.Services;

public interface ITransactionService
{
    Task<TransactionDto> CreateAsync(CreateTransactionDto dto, CancellationToken cancellationToken = default);
    Task<TransactionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TransactionDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, UpdateTransactionDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}