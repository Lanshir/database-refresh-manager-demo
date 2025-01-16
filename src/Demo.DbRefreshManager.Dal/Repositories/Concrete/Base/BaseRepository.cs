using Demo.DbRefreshManager.Dal.Context;
using Demo.DbRefreshManager.Dal.Repositories.Abstract.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Demo.DbRefreshManager.Dal.Repositories.Concrete.Base;

/// <summary>
/// Базовый репозиторий.
/// </summary>
/// <inheritdoc cref="RepositoryBase" />
public class BaseRepository<TEntity>(
    IDbContextFactory<AppDbContext> contextFactory
    ) : IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Фабрика контекстов EF.
    /// </summary>
    protected IDbContextFactory<AppDbContext> ContextFactory { get; } = contextFactory;

    protected virtual IQueryable<TEntity> Get()
    {
        var ctx = ContextFactory.CreateDbContext();

        return ctx.Set<TEntity>();
    }

    protected virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        var ctx = ContextFactory.CreateDbContext();
        var entry = await ctx.AddAsync(entity);

        await ctx.SaveChangesAsync();

        return entry.Entity;
    }

    protected virtual async Task CreateManyAsync(List<TEntity> entities)
    {
        var ctx = await ContextFactory.CreateDbContextAsync();

        await ctx.AddRangeAsync(entities);
        await ctx.SaveChangesAsync();
    }

    protected virtual async Task UpdatePropsAsync(
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
        Expression<Func<TEntity, bool>> where)
    {
        var ctx = await ContextFactory.CreateDbContextAsync();

        await ctx.Set<TEntity>()
            .Where(where)
            .ExecuteUpdateAsync(setPropertyCalls);
    }

    protected virtual async Task<IQueryable<TEntity>> UpdatePropsReturningAsync(
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
        Expression<Func<TEntity, bool>> where)
    {
        var ctx = await ContextFactory.CreateDbContextAsync();

        await ctx.Set<TEntity>()
            .Where(where)
            .ExecuteUpdateAsync(setPropertyCalls);

        var updated = ctx.Set<TEntity>().Where(where);

        return updated;
    }

    protected virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> where)
    {
        var ctx = await ContextFactory.CreateDbContextAsync();

        await ctx.Set<TEntity>().Where(where).ExecuteDeleteAsync();
    }
}
