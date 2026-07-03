using FinanceTracker.Application.Abstractions.Services;
using FinanceTracker.Application.DTOs.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionsController(ITransactionService transactionService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var transactions = await transactionService.GetAllAsync(cancellationToken);
        return Ok(transactions);
    }
    
    [HttpGet("{id:guid}")]
    [ActionName("GetTransactionById")]
    public async Task<ActionResult<TransactionDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var transaction = await transactionService.GetByIdAsync(id, cancellationToken);
        
        if (transaction == null)
        {
            return NotFound(new { messge = $"Transaction not found" });
        }
        
        return Ok(transaction);
    }

    [HttpPost]
    public async Task<ActionResult<TransactionDto>> UpsertAsync([FromBody] CreateTransactionDto createTransactionDto,
        CancellationToken cancellationToken = default)
    {
        var transaction = await transactionService.CreateAsync(createTransactionDto, cancellationToken);
        
        return CreatedAtAction("GetTransactionById", new { id = transaction.Id }, transaction);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var isDeleted = await transactionService.DeleteAsync(id, cancellationToken);
        
        if (!isDeleted)
        {
            return NotFound(new { messge = $"Transaction not found" });
        }
        
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateAsync(Guid id, [FromBody] UpdateTransactionDto updateTransactionDto,
        CancellationToken cancellationToken = default)
    {
        await transactionService.UpdateAsync(id, updateTransactionDto, cancellationToken);
        return NoContent();
    }
}