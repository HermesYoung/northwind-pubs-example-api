using Microsoft.Extensions.DependencyInjection;
using Repositories.Abstract;

namespace Repositories.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IProductRepository, ProductRepository>();
        return services;
    }
}