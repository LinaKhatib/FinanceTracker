using System.ComponentModel.Design;
using FinanceTracker.Application.Abstractions.Services;
using FinanceTracker.Application.Implementations.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceTracker.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ITransactionService, TransactionService>();
        
        return services;
    }
}