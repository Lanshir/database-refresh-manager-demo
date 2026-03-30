using Demo.DbRefreshManager.Dal.Context;
using Demo.DbRefreshManager.Dal.Repositories.Abstract.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Demo.DbRefreshManager.Dal.Repositories.Concrete.Base;

/// <summary>
/// Базовый репозиторий.
/// </summary>
public class BaseRepository<TEntity>(
    IDbContextFactory<AppDbContext> contextFactory
    ) : IDisposable, IRepository<TEntity> where TEntity : class
{
    private readonly List<AppDbContext> _contextsToDispose = [];

    /// <summary>
    /// Фабрика контекстов EF.
    /// </summary>
    protected IDbContextFactory<AppDbContext> ContextFactory { get; } = contextFactory;

    protected virtual IQueryable<TEntity> GetQueriable()
    {
        var ctx = ContextFactory.CreateDbContext();
        _contextsToDispose.Add(ctx);

        return ctx.Set<TEntity>();
    }

    protected virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        using var ctx = await ContextFactory.CreateDbContextAsync();
        var entry = await ctx.AddAsync(entity);

        await ctx.SaveChangesAsync();

        return entry.Entity;
    }

    protected virtual async Task CreateManyAsync(List<TEntity> entities)
    {
        using var ctx = await ContextFactory.CreateDbContextAsync();

        await ctx.AddRangeAsync(entities);
        await ctx.SaveChangesAsync();
    }

    protected virtual async Task UpdatePropsAsync(
        Action<UpdateSettersBuilder<TEntity>> setPropertyCalls,
        Expression<Func<TEntity, bool>> where)
    {
        using var ctx = await ContextFactory.CreateDbContextAsync();

        await ctx.Set<TEntity>()
            .Where(where)
            .ExecuteUpdateAsync(setPropertyCalls);
    }

    protected virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> where)
    {
        using var ctx = await ContextFactory.CreateDbContextAsync();

        await ctx.Set<TEntity>().Where(where).ExecuteDeleteAsync();
    }

    public void Dispose()
    {
        _contextsToDispose.ForEach(ctx => ctx.Dispose());
        GC.SuppressFinalize(this);
    }
}
