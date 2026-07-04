using FinanceTracker.Application.Validators.Categories;
using FinanceTracker.Application.Validators.Transactions;
using FluentValidation;

namespace FinanceTracker.API.Extensions;

public static class ValidatorExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateCategoryDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateCategoryDtoValidator>();
        
        services.AddValidatorsFromAssemblyContaining<CreateTransactionDtoValidator>();       
        services.AddValidatorsFromAssemblyContaining<UpdateTransactionDtoValidator>();

        return services;
    }

}