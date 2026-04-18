namespace Demo.DbRefreshManager.Core.Handlers;

/// <summary>
/// Основа-маркер всех обработчиков команд/запросов.
/// </summary>
public interface IHandlerBase { }

public interface IHandler : IHandlerBase
{
    public void Handle();
}

public interface IHandler<TResult> : IHandlerBase
{
    public TResult Handle();
}

public interface IHandler<TResult, TInput> : IHandlerBase
{
    public TResult Handle(TInput input);
}

public interface IAsyncHandler : IHandlerBase
{
    public Task HandleAsync(CancellationToken cancellationToken);
}

public interface IAsyncHandler<TResult> : IHandlerBase
{
    public Task<TResult> HandleAsync(CancellationToken cancellationToken);
}

public interface IAsyncHandler<TResult, TInput> : IHandlerBase
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken cancellationToken);
}