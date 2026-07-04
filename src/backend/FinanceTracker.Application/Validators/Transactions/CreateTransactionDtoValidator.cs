using System.Data;
using FinanceTracker.Application.DTOs.Transactions;
using FluentValidation;

namespace FinanceTracker.Application.Validators.Transactions;

public class CreateTransactionDtoValidator : AbstractValidator<CreateTransactionDto>
{
    public CreateTransactionDtoValidator()
    {
        RuleFor(t => t.Amount)
            .NotEmpty().WithMessage("Сумма транзакции обязательна для заполнения.")
            .GreaterThan(0).WithMessage("Сумма транжакции не может быть отрицательной.");

        RuleFor(t => t.Description)
            .MaximumLength(500).WithMessage("Описание не может привышать 500 символов.");

        RuleFor(t => t.CategoryId)
            .NotEmpty().WithMessage("Необходимо выбрать категорию");
    }
}