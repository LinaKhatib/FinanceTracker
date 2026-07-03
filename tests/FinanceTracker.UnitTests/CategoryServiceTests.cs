using FinanceTracker.Application.Abstractions.Repositories;
using FinanceTracker.Application.DTOs.Categories;
using FinanceTracker.Application.Implementations.Services;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FluentAssertions;
using Moq;

namespace FinanceTracker.UnitTests;

public class CategoryServiceTests
{
    #region CreateAsync
    [Fact]
    public async Task CreateAsync_ValidDto_ShouldReturnCategoryDtoAndCallRepository()
    {
        var categoryRepositoryMock = new Mock<IRepository<Category>>();
        var cancellationToken = CancellationToken.None;

        var inputDto = new CreateCategoryDto()
        {
            Name = "Кофе",
            Type = "Expense"
        };
        
        categoryRepositoryMock
            .Setup(repo => repo.CreateAsync(It.IsAny<Category>(),  cancellationToken))
            .Returns(Task.CompletedTask);
        
        var service = new CategoryService(categoryRepositoryMock.Object);
        
        
        var result = await service.CreateAsync(inputDto, cancellationToken);
        
        
        result.Should().NotBeNull();
        result.Name.Should().Be(inputDto.Name);
        result.Id.Should().NotBeEmpty();
        
        categoryRepositoryMock.Verify(repo => 
            repo.CreateAsync(
                It.Is<Category>(c => 
                    c.Name == inputDto.Name && 
                    c.Type.ToString() == inputDto.Type &&
                    c.Id != Guid.Empty), 
                cancellationToken), Times.Once);
    }
    #endregion
    
    #region GetByIdAsync
    [Fact]
    public async Task GetByIdAsync_ElementExists_ShouldReturnCorrectDtoAndCallRepository()
    {
        var categoryRepositoryMock = new Mock<IRepository<Category>>();
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
        
        var service = new CategoryService(categoryRepositoryMock.Object);
        
        
        var result = await service.GetByIdAsync(existingCategory.Id, cancellationToken);
        
        
        result.Should().NotBeNull();
        result.Id.Should().Be(existingCategory.Id);
        result.Name.Should().Be("Транспорт");
        
        categoryRepositoryMock.Verify(repo => repo.GetByIdAsync(existingCategory.Id, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ElementDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        var categoryRepositoryMock = new Mock<IRepository<Category>>();
        var cancellationToken = CancellationToken.None;
        
        var nonExistingId = Guid.NewGuid();

        categoryRepositoryMock
            .Setup(repo => repo.GetByIdAsync(nonExistingId, cancellationToken))
            .ReturnsAsync((Category)null!);
        
        var service = new CategoryService(categoryRepositoryMock.Object);
        
        
        Func<Task> act = async () => await service.GetByIdAsync(nonExistingId, cancellationToken);
        
        
        await act.Should().ThrowAsync<KeyNotFoundException>();
        
        
        categoryRepositoryMock.Verify(repo => repo.GetByIdAsync(nonExistingId, cancellationToken), Times.Once);
    }
    #endregion

    #region GetAllAsync
    [Fact]
    public async Task GetAllAsync_CategoriesExist_ShouldReturnListOfCategoryDtos()
    {
        var categoryRepositoryMock = new Mock<IRepository<Category>>();
        var cancellationToken = CancellationToken.None;

        var categoriesFromDb = new List<Category>
        {
            new Category { Id = Guid.NewGuid(), Name = "Продукты", Type = TransactionType.Expense },
            new Category { Id = Guid.NewGuid(), Name = "Зарплата", Type = TransactionType.Income }
        };
        
        categoryRepositoryMock
            .Setup(repo => repo.GetAllAsync(cancellationToken))
            .ReturnsAsync(categoriesFromDb);

        var service = new CategoryService(categoryRepositoryMock.Object);

        
        var result = await service.GetAllAsync(cancellationToken);
        
        
        result.Should().NotBeNull();
        result.Should().HaveCount(2); 
        
        var firstDto = result.First();
        firstDto.Id.Should().Be(categoriesFromDb[0].Id);
        firstDto.Name.Should().Be("Продукты");

        categoryRepositoryMock.Verify(repo => repo.GetAllAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_NoCategoriesExist_ShouldReturnEmptyCollection()
    {
        // 1. Arrange
        var categoryRepositoryMock = new Mock<IRepository<Category>>();
        var cancellationToken = CancellationToken.None;

        categoryRepositoryMock
            .Setup(repo => repo.GetAllAsync(cancellationToken))
            .ReturnsAsync(new List<Category>());

        var service = new CategoryService(categoryRepositoryMock.Object);

        
        var result = await service.GetAllAsync(cancellationToken);

        
        result.Should().NotBeNull();
        result.Should().BeEmpty(); 
        
        categoryRepositoryMock.Verify(repo => repo.GetAllAsync(cancellationToken), Times.Once);
    }
    #endregion
    
    #region UpdateAsync
    [Fact]
    public async Task UpdateAsync_ValidIdAndDto_ShouldVerifyExistenceAndCallRepository()
    {
        var categoryRepositoryMock = new Mock<IRepository<Category>>();
        var cancellationToken = CancellationToken.None;

        
        var inputDto = new UpdateCategoryDto()
        {
            Name = "Подписки"
        };
        
        var existingCategory = new Category 
        { 
            Id = Guid.NewGuid(), 
            Name = "Старое название", 
            Type = TransactionType.Expense 
        };

        categoryRepositoryMock
            .Setup(repo => repo.GetByIdAsync(existingCategory.Id, cancellationToken))
            .ReturnsAsync(existingCategory);
        
        categoryRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Category>(),  cancellationToken))
            .Returns(Task.CompletedTask);
        
        var service = new CategoryService(categoryRepositoryMock.Object);


        await service.UpdateAsync(existingCategory.Id, inputDto, cancellationToken);
        
        
        categoryRepositoryMock.Verify(repo => repo.GetByIdAsync(existingCategory.Id, cancellationToken), Times.Once);
        
        categoryRepositoryMock.Verify(repo => 
            repo.UpdateAsync(It.Is<Category>(c => 
                c.Id == existingCategory.Id &&
                c.Name == inputDto.Name), cancellationToken), Times.Once);
    }
    #endregion

    #region DeleteAsync
    [Fact]
    public async Task DeleteAsync_ElementExists_ShouldDeleteAndReturnTrue()
    {
        var categoryRepositoryMock = new Mock<IRepository<Category>>();
        var cancellationToken = CancellationToken.None;
    
        var existingCategory = new Category 
        { 
            Id = Guid.NewGuid(), 
            Name = "Развлечения", 
            Type = TransactionType.Expense
        };

        categoryRepositoryMock
            .Setup(repo => repo.GetByIdAsync(existingCategory.Id, cancellationToken))
            .ReturnsAsync(existingCategory);

        categoryRepositoryMock
            .Setup(repo => repo.DeleteAsync(existingCategory, cancellationToken))
            .Returns(Task.CompletedTask);

        var service = new CategoryService(categoryRepositoryMock.Object);

        
        var result = await service.DeleteAsync(existingCategory.Id, cancellationToken);

        
        result.Should().BeTrue();
        
        categoryRepositoryMock.Verify(repo => repo.GetByIdAsync(existingCategory.Id, cancellationToken), Times.Once);
        
        categoryRepositoryMock.Verify(
            repo => repo.DeleteAsync(It.Is<Category>(c => c.Id == existingCategory.Id), cancellationToken), 
            Times.Once);
    }
    
    [Fact]
    public async Task DeleteAsync_ElementDoesNotExist_ShouldNotCallDeleteAndReturnFalse()
    {
        var categoryRepositoryMock = new Mock<IRepository<Category>>();
        var cancellationToken = CancellationToken.None;
        var nonExistingId = Guid.NewGuid();

        categoryRepositoryMock
            .Setup(repo => repo.GetByIdAsync(nonExistingId, cancellationToken))
            .ReturnsAsync((Category)null!);

        var service = new CategoryService(categoryRepositoryMock.Object);

        
        var result = await service.DeleteAsync(nonExistingId, cancellationToken);

        
        result.Should().BeFalse(); 
        
        
        categoryRepositoryMock.Verify(
            repo => repo.DeleteAsync(It.IsAny<Category>(), cancellationToken), 
            Times.Never);
    }
    #endregion
}