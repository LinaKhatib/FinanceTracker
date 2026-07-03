using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.DbContexts;
using FinanceTracker.Application.Abstractions.Repositories;
using FinanceTracker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceTracker.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRepository<Category>, CategoryRepository>();
        
        services.AddScoped<IRepository<Transaction>, TransactionRepository>();

        services.AddDbContext<TransactionsDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("PostgresDb");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Строка подключения 'PostgresDb' не найдена в файле конфигурации!");
            }

            options.UseNpgsql(connectionString);
        });
        return services;
    }
}