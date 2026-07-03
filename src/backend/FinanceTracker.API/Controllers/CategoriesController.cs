using FinanceTracker.Application.Abstractions.Services;
using FinanceTracker.Application.DTOs.Categories;
using FinanceTracker.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await categoryService.GetAllAsync(cancellationToken);
        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    [ActionName("GetCategoryById")]
    public async Task<ActionResult<CategoryDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await categoryService.GetByIdAsync(id, cancellationToken);

        if (category == null)
        {
            return NotFound(new { messge = $"Category not found" });
        }

        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateAsync([FromBody] CreateCategoryDto dto,
        CancellationToken cancellationToken = default)
    {
        var category = await categoryService.CreateAsync(dto, cancellationToken);

        return CreatedAtAction("GetCategoryById", new { id = category.Id }, category);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var isDeleted = await categoryService.DeleteAsync(id, cancellationToken);
        if (!isDeleted)
        {
            return NotFound(new { messge = $"Category not found" });
        }
        
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CategoryDto>> UpdateAsync(Guid id,[FromBody] UpdateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        await categoryService.UpdateAsync(id, dto, cancellationToken);
        return NoContent();
    }
}