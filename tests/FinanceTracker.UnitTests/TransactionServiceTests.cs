using FinanceTracker.Application.Abstractions.Repositories;
using FinanceTracker.Application.DTOs.Transactions;
using FinanceTracker.Application.Implementations.Services;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FluentAssertions;
using Moq;

namespace FinanceTracker.UnitTests;

public class TransactionServiceTests
{
    #region CreateAsync
    [Fact]
    public async Task CreateAsync_ValidDtoForTheExistingCategory_ShouldReturnTransactionDtoAndCallRepository()
    {
        var categoryRepositoryMock = new Mock<IRepository<Category>>();
        var transactionRepositoryMock = new Mock<IRepository<Transaction>>();
        var cancellationToken = CancellationToken.None;
    
        
        var existingCategory = new Category 
        { 
            Id = Guid.NewGuid(), 
            Name = "Транспорт", 
            Type = TransactionType.Expense 
        };
        
        categoryRepositoryMock
            .Setup(repo => repo.GetByIdAsync(existingCategory.Id, cancellationToken))
            .ReturnsAsync(existingCategory);
        
        var inputDto = new CreateTransactionDto()
        {
            Amount = 55,
            Description = "билет на автобус",
            CategoryId = existingCategory.Id,
        };
        
        transactionRepositoryMock
            .Setup(repo => repo.CreateAsync(It.IsAny<Transaction>(),  cancellationToken))
            .Returns(Task.CompletedTask);
        
        var service = new TransactionService(transactionRepositoryMock.Object, categoryRepositoryMock.Object);
        
        
        var result = await service.CreateAsync(inputDto, cancellationToken);
        
        
        result.Should().NotBeNull();
        result.Amount.Should().Be(inputDto.Amount);
        result.Id.Should().NotBeEmpty();
        
        transactionRepositoryMock.Verify(repo => 
            repo.CreateAsync(
                It.Is<Transaction>(t => 
                    t.Amount == inputDto.Amount && 
                    t.Description == inputDto.Description &&
                    t.CategoryId == inputDto.CategoryId &&
                    t.Id != Guid.Empty), 
                cancellationToken), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ValidDtoForANonExistingCategory_ShouldReturnTransactionDtoAndCallRepository()
    {
        var categoryRepositoryMock = new Mock<IRepository<Category>>();
        var transactionRepositoryMock = new Mock<IRepository<Transaction>>();
        var cancellationToken = CancellationToken.None;
        
        var nonExistingIdCategory = Guid.NewGuid();

        categoryRepositoryMock
            .Setup(repo => repo.GetByIdAsync(nonExistingIdCategory, cancellationToken))
            .ReturnsAsync((Category)null!);
        
        var inputDto = new CreateTransactionDto()
        {
            Amount = 55,
            Description = "билет на автобус",
            CategoryId = nonExistingIdCategory
        };
        
        var service = new TransactionService(transactionRepositoryMock.Object, categoryRepositoryMock.Object);
        
        
        Func<Task> act = async () => await service.CreateAsync(inputDto, cancellationToken);

        await act.Should().ThrowAsync<KeyNotFoundException>();
        
        transactionRepositoryMock.Verify(repo =>
                repo.CreateAsync(It.IsAny<Transaction>(), cancellationToken), Times.Never);
    }
    #endregion
    
    #region GetByIdAsync
    [Fact]
    public async Task GetByIdAsync_ExistingId_ShouldReturnTransactionDto()
    {
        var categoryRepositoryMock = new Mock<IRepository<Category>>();
        var transactionRepositoryMock = new Mock<IRepository<Transaction>>();
        var cancellationToken = CancellationToken.None;

        var existingCategory = new Category()
        {
            Id = Guid.NewGuid(),
            Name = "Транспорт",
            Type = TransactionType.Expense
        };
        
        var existingTransaction = new Transaction()
        {
            Id = Guid.NewGuid(),
            Amount = 55,
            Description = "билет на автобус",
            CategoryId = existingCategory.Id,
            Date = DateTime.UtcNow,
        };
        
        categoryRepositoryMock
            .Setup(repo => repo.GetByIdAsync(existingCategory.Id, cancellationToken))
            .ReturnsAsync(existingCategory);
        
        transactionRepositoryMock
            .Setup(repo => repo.GetByIdAsync(existingTransaction.Id, cancellationToken))
            .ReturnsAsync(existingTransaction);
        
        var service = new TransactionService(transactionRepositoryMock.Object, categoryRepositoryMock.Object);
        
        
        var result = await service.GetByIdAsync(existingTransaction.Id, cancellationToken);
        
        result.Id.Should().Be(existingTransaction.Id);
        result.Amount.Should().Be(existingTransaction.Amount);
        result.Description.Should().Be(existingTransaction.Description);
        result.CategoryId.Should().Be(existingTransaction.CategoryId);
        
        transactionRepositoryMock.Verify(repo => repo.GetByIdAsync(existingTransaction.Id, cancellationToken), Times.Once);
    }
    
    [Fact]
    public async Task GetByIdAsync_NonExistingId_ShouldThrowNotFoundException()
    {
        var categoryRepositoryMock = new Mock<IRepository<Category>>();
        var transactionRepositoryMock = new Mock<IRepository<Transaction>>();
        var cancellationToken = CancellationToken.None;
        
        var nonExistingTransactionId = Guid.NewGuid();

        transactionRepositoryMock
            .Setup(repo => repo.GetByIdAsync(nonExistingTransactionId, cancellationToken))
            .ReturnsAsync((Transaction)null!);
        
        var service = new TransactionService(transactionRepositoryMock.Object, categoryRepositoryMock.Object);
        
        var result = await  service.GetByIdAsync(nonExistingTransactionId, cancellationToken);
        
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingCategoryId_ShouldThrowKeyNotFoundException()
    {
        var categoryRepositoryMock = new Mock<IRepository<Category>>();
        var transactionRepositoryMock = new Mock<IRepository<Transaction>>();
        var cancellationToken = CancellationToken.None;
        
        var nonExistingId = Guid.NewGuid();

        var existingTransaction = new Transaction()
        {
            Id = Guid.NewGuid(),
            Amount = 55,
            Description = "билет на автобус",
            CategoryId = nonExistingId,
            Date = DateTime.UtcNow,
        };
        
        transactionRepositoryMock
            .Setup(repo => repo.GetByIdAsync(existingTransaction.Id, cancellationToken))
            .ReturnsAsync(existingTransaction);
        
        categoryRepositoryMock
            .Setup(repo => repo.GetByIdAsync(nonExistingId, cancellationToken))
            .ReturnsAsync((Category)null!);

        var service = new TransactionService(transactionRepositoryMock.Object, categoryRepositoryMock.Object);

        
        Func<Task> act = async () => await service.GetByIdAsync(existingTransaction.Id, cancellationToken);

        
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Category {nonExistingId} not found");
    }
    
    #endregion
    
    #region GetAllAsync

    [Fact]
    public async Task GetAllAsync_WhenTransactionsExist_ShouldReturnAllTransactionsOrderedByDate()
    { 
        var transactionRepositoryMock = new Mock<IRepository<Transaction>>();
        var categoryRepositoryMock = new Mock<IRepository<Category>>();
        var cancellationToken = CancellationToken.None;

        var category1 = new Category { Name = "Продукты" };
        var category2 = new Category { Name = "Транспорт" };

        var fakeTransactions = new List<Transaction>
        {
            new Transaction 
            { 
                Id = Guid.NewGuid(), 
                Amount = 500, 
                Description = "Супермаркет", 
                Date = DateTime.UtcNow.AddDays(-1),
                Category = category1,
                CategoryId = Guid.NewGuid()
            },
            new Transaction 
            { 
                Id = Guid.NewGuid(), 
                Amount = 55, 
                Description = "Автобус", 
                Date = DateTime.UtcNow,
                Category = category2,
                CategoryId = Guid.NewGuid()
            }
        };
        
        transactionRepositoryMock
            .Setup(repo => repo.GetAllAsync(cancellationToken))
            .ReturnsAsync(fakeTransactions);
        
        var service = new TransactionService(transactionRepositoryMock.Object, categoryRepositoryMock.Object);
        
        
        var result = await service.GetAllAsync(cancellationToken);
        
        
        result.Should().NotBeNull();
        result.Should().HaveCount(fakeTransactions.Count); 
        
        var firstDto = result.First();
        var firstFake = fakeTransactions.First();

        firstDto.Id.Should().Be(firstFake.Id);
        firstDto.Amount.Should().Be(firstFake.Amount);
        firstDto.Description.Should().Be(firstFake.Description);
        firstDto.CategoryName.Should().Be(firstFake.Category.Name);
    }
    
    [Fact]
    public async Task GetAllAsync_WhenNoTransactionsExist_ShouldReturnEmptyCollection()
    {
        var transactionRepositoryMock = new Mock<IRepository<Transaction>>();
        var categoryRepositoryMock = new Mock<IRepository<Category>>();
        var cancellationToken = CancellationToken.None;

        transactionRepositoryMock
            .Setup(repo => repo.GetAllAsync(cancellationToken))
            .ReturnsAsync(new List<Transaction>()); 

        var service = new TransactionService(transactionRepositoryMock.Object, categoryRepositoryMock.Object);

        
        var result = await service.GetAllAsync(cancellationToken);

            
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
    #endregion

    #region UpdateAsync

    [Fact]
    public async Task UpdateAsync_ExistingTransactionAndValidDto_ShouldUpdateTransaction()
    {
        var transactionRepositoryMock = new Mock<IRepository<Transaction>>();
        var categoryRepositoryMock = new Mock<IRepository<Category>>(); 
        var cancellationToken = CancellationToken.None;

        var existingTransaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Amount = 100,
            Description = "Старое описание",
            Date = DateTime.UtcNow,
            CategoryId = Guid.NewGuid(),
        };
        
        transactionRepositoryMock
            .Setup(repo => repo.GetByIdAsync(existingTransaction.Id, cancellationToken))
            .ReturnsAsync(existingTransaction);

        var newCategoryId = Guid.NewGuid();
        
        var inputDto = new UpdateTransactionDto
        {
            Amount = 250,
            Description = null,
            Date = null,
            CategoryId = newCategoryId,
        };
        
        var service = new TransactionService(transactionRepositoryMock.Object, categoryRepositoryMock.Object);
        
        
        await service.UpdateAsync(existingTransaction.Id, inputDto, cancellationToken);
        
        
        transactionRepositoryMock.Verify(repo =>
                repo.UpdateAsync(It.Is<Transaction>(t =>
                        t.Id == existingTransaction.Id &&
                        t.Amount == 250 &&
                        t.Description == existingTransaction.Description &&
                        t.Date == existingTransaction.Date &&
                        t.CategoryId == newCategoryId
                ), cancellationToken), 
            Times.Once);
    }
    
    [Fact]
    public async Task UpdateAsync_NonExistingTransaction_ShouldThrowKeyNotFoundExceptionAndNotCallUpdate()
    {
        var transactionRepositoryMock = new Mock<IRepository<Transaction>>();
        var categoryRepositoryMock = new Mock<IRepository<Category>>();
        var cancellationToken = CancellationToken.None;
    
        var nonExistingId = Guid.NewGuid();
        var updateDto = new UpdateTransactionDto { Amount = 500 };

        transactionRepositoryMock
            .Setup(repo => repo.GetByIdAsync(nonExistingId, cancellationToken))
            .ReturnsAsync((Transaction)null!);

        var service = new TransactionService(transactionRepositoryMock.Object, categoryRepositoryMock.Object);

        
        Func<Task> act = async () => await service.UpdateAsync(nonExistingId, updateDto, cancellationToken);

        
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Transaction with ID {nonExistingId} was not found.");

        transactionRepositoryMock.Verify(repo => 
                repo.UpdateAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), 
            Times.Never);
    }
    #endregion
    
    #region DeleteAsync
    [Fact]
    public async Task DeleteAsync_TransactionExists_ShouldDeleteAndReturnTrue()
    {
        var transactionRepositoryMock = new Mock<IRepository<Transaction>>();
        var categoryRepositoryMock = new Mock<IRepository<Category>>();
        var cancellationToken = CancellationToken.None;

        var existingTransaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Amount = 500,
            Description = "Кофе"
        };
        
        transactionRepositoryMock
            .Setup(repo => repo.GetByIdAsync(existingTransaction.Id, cancellationToken))
            .ReturnsAsync(existingTransaction);
        
        var service = new TransactionService(transactionRepositoryMock.Object, categoryRepositoryMock.Object);
        
        
        var result = await service.DeleteAsync(existingTransaction.Id, cancellationToken);
        
        result.Should().BeTrue();

        transactionRepositoryMock.Verify(repo =>
            repo.DeleteAsync(existingTransaction, cancellationToken), Times.Once);
    }
    
    [Fact]
    public async Task DeleteAsync_TransactionDoesNotExist_ShouldReturnFalseAndNotCallDelete()
    {
        var transactionRepositoryMock = new Mock<IRepository<Transaction>>();
        var categoryRepositoryMock = new Mock<IRepository<Category>>();
        var cancellationToken = CancellationToken.None;

        var  nonExistingId = Guid.NewGuid();
        
        transactionRepositoryMock
            .Setup(repo => repo.GetByIdAsync(nonExistingId, cancellationToken))
            .ReturnsAsync((Transaction)null!);
        
        var service = new TransactionService(transactionRepositoryMock.Object, categoryRepositoryMock.Object);
        
        
        var result = await service.DeleteAsync(nonExistingId, cancellationToken);
        
        result.Should().BeFalse();

        transactionRepositoryMock.Verify(repo =>
            repo.DeleteAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Never);
    }
    #endregion
}