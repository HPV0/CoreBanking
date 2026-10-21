using CoreBanking.Infrastructure.Data.Context;
using CoreBanking.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace CoreBanking.Infrastructure.Persistence
{
    public static class DatabaseInitializerExtensions
    {
        public static async Task InitializeDatabaseAsync(
            this IServiceProvider services)
        {
            await using var scope = services.CreateAsyncScope();

            var context = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync();

            await DbInitializer.SeedAsync(context);
        }
    }

}
