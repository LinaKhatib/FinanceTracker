using System.Data;
using FinanceTracker.Application.DTOs.Transactions;
using FluentValidation;

namespace FinanceTracker.Application.Validators.Transactions;

public class UpdateTransactionDtoValidator : AbstractValidator<UpdateTransactionDto>
{
    public UpdateTransactionDtoValidator()
    {
        When(t => t.Amount != null, () =>
        {
            RuleFor(t => t.Amount)
                .GreaterThan(0).WithMessage("Сумма транзакции не может быть отрицательной.");
        });

        When(t => t.Description != null, () =>
        {
            RuleFor(t => t.Description)
                .MaximumLength(500).WithMessage("Описание не может привышать 500 символов.");
        });

        When(t => t.Date != null, () =>
        {
            RuleFor(t => t.Date)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Дата транзакции не может быть в будущем.");
        });

        When(t => t.CategoryId != null, () =>
        {
            RuleFor(t => t.CategoryId)
                .NotEmpty().WithMessage("Необходимо выбрать категорию");
        });
    }
}