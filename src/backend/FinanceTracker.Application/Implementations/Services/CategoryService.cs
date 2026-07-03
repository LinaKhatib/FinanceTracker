using FinanceTracker.Application.Abstractions.Repositories;
using FinanceTracker.Application.Abstractions.Services;
using FinanceTracker.Application.DTOs.Categories;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Implementations.Services;

public class CategoryService (IRepository<Category> categoryRepository) : ICategoryService
{
    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<TransactionType>(dto.Type, true, out var categoryType))
        {
            throw new ArgumentException($"Некорректный тип категории: {dto.Type}");
        }

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Type = categoryType
        };
        
        await categoryRepository.CreateAsync(category, cancellationToken);
        
        return new CategoryDto
        {
            Id =  category.Id,
            Name =  category.Name,
            Type = category.Type.ToString()
        };
    }

    public async Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(id, cancellationToken);

        if (category == null)
        {
            throw new KeyNotFoundException($"Категория с Id {id} не найдена.");
        }
        
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Type = category.Type.ToString()
        };
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories =  await categoryRepository.GetAllAsync(cancellationToken);

        return categories.Select(category => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Type = category.Type.ToString()
        });
    }

    public async Task UpdateAsync(Guid id, UpdateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(id, cancellationToken);
        
        if (category is null)
        {
            throw new KeyNotFoundException($"Transaction with ID {id} was not found.");
        }

        category.Name = dto.Name;
        
        await categoryRepository.UpdateAsync(category, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(id, cancellationToken);

        if (category is null) return false;
        
        await categoryRepository.DeleteAsync(category, cancellationToken);
        return true;
    }
}