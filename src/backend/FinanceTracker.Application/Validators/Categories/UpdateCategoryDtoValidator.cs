using FinanceTracker.Application.DTOs.Categories;
using FluentValidation;

namespace FinanceTracker.Application.Validators.Categories;

public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
{
    public  UpdateCategoryDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Название категории обязательно для заполнения.")
            .MaximumLength(100).WithMessage("Название категории не может привышать 100 символов.");
    }
}