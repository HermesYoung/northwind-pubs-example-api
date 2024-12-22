using DatabaseContext.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DatabaseContext.Extensions;

public static class DatabaseContextExtensions
{
    public static IServiceCollection AddDatabaseContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<NorthwindPubsDbContext>(builder =>
        {
            builder.UseSqlServer(connectionString);
        });
        return services;
    }
}