namespace Demo.DbRefreshManager.Application.Repositories.Base;

/// <summary>
/// Интерфейс базового репозитория.
/// </summary>
/// <typeparam name="TEntity">Тип сущности репозитория.</typeparam>
public interface IRepository<TEntity> where TEntity : class { }
