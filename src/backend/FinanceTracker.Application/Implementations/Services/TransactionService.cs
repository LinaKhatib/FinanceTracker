using FinanceTracker.Application.Abstractions.Repositories;
using FinanceTracker.Application.Abstractions.Services;
using FinanceTracker.Application.DTOs.Transactions;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Implementations.Services;

public class TransactionService(IRepository<Transaction> transactionRepository, IRepository<Category> categoryRepository) : ITransactionService
{
    public async Task<TransactionDto> CreateAsync(CreateTransactionDto dto, CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(dto.CategoryId, cancellationToken);
        
        if (category is null)
        {
            throw new KeyNotFoundException($"Category {dto.CategoryId} not found");
        }
        
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Amount = dto.Amount,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            Date = DateTime.UtcNow,
            Category = category
        };
        
        await transactionRepository.CreateAsync(transaction, cancellationToken);

        return new TransactionDto
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Description = transaction.Description,
            CategoryId = transaction.CategoryId,
            Date = transaction.Date,
            CategoryName = category.Name
        };
    }

    public async Task<TransactionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var transaction = await transactionRepository.GetByIdAsync(id, cancellationToken);
        
        if (transaction is null) return null;
        
        var category = await categoryRepository.GetByIdAsync(transaction.CategoryId, cancellationToken);
        
        if (category is null) throw new KeyNotFoundException($"Category {transaction.CategoryId} not found");
        
        return new TransactionDto
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Description = transaction.Description,
            CategoryId = transaction.CategoryId,
            Date = transaction.Date,
            CategoryName = category.Name
        };
    }

    public async Task<IEnumerable<TransactionDto>> GetAllAsync(CancellationToken cancellationToken = default) // пока выгрузка только по дате без выбора способа соритовки транзакций
    {
        var transactions = await transactionRepository.GetAllAsync(cancellationToken);
        
        return transactions.Select(t => new TransactionDto
        {
            Id = t.Id,
            Amount = t.Amount,
            Description = t.Description,
            CategoryId = t.CategoryId,
            Date = t.Date,
            CategoryName = t.Category.Name
        });
    }

    public async Task UpdateAsync(Guid id, UpdateTransactionDto dto, CancellationToken cancellationToken = default)
    {
         
        var transaction = await transactionRepository.GetByIdAsync(id, cancellationToken);

        if (transaction is null)
        {
            throw new KeyNotFoundException($"Transaction with ID {id} was not found.");
        }

        if (dto.Amount is not null)
        {
            transaction.Amount = dto.Amount.Value;
        }
        
        if (dto.Description is not null)
        {
            transaction.Description = dto.Description;
        }
        
        if (dto.Date is not null)
        {
            transaction.Date = dto.Date.Value;
        }
        
        if (dto.CategoryId is not null)
        {
            transaction.CategoryId = (Guid)dto.CategoryId;
        }
        
        await transactionRepository.UpdateAsync(transaction, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var transaction = await transactionRepository.GetByIdAsync(id, cancellationToken);

        if (transaction is null) return false;
        
        await transactionRepository.DeleteAsync(transaction, cancellationToken);
        return true;
    }
}