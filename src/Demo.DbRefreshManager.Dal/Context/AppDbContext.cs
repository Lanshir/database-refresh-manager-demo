using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Dal.Context;

/// <summary>
/// Контекст Entity Framework приложения.
/// </summary>
public class AppDbContext : DbContext
{
    /// <inheritdoc cref="AppDbContext" />
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        // Создание демонстрационной БД с данными.
        if (Database.EnsureCreated())
        {
            Database.ExecuteSqlRaw(DemoDataQueries.DbGroups);
            Database.ExecuteSqlRaw(DemoDataQueries.DbRefreshJobs);
            Database.ExecuteSqlRaw(DemoDataQueries.UserRoles);
            Database.ExecuteSqlRaw(DemoDataQueries.GroupsRoles);
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Все даты sqlite по ум. в utc.
        configurationBuilder
            .Properties<DateTime>()
            .HaveConversion<ValueConverters.DateTimeToUtcConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
