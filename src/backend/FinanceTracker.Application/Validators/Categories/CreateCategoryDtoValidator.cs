using FinanceTracker.Application.DTOs.Categories;
using FluentValidation;

namespace FinanceTracker.Application.Validators.Categories;

public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
{
    public  CreateCategoryDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Название категории обязательна для заполнения.")
            .MaximumLength(100).WithMessage("Название категории не может привышать 100 символов.");

        RuleFor(c => c.Type)
            .NotEmpty().WithMessage("Тип категории обязателен для заполнения.")
            .Must(type => type == "Income" || type == "Expense").WithMessage("Тип категории должен быть 'Income' или 'Expense'");

    }
}