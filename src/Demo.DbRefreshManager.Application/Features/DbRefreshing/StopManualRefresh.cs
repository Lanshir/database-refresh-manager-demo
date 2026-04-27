using Demo.DbRefreshManager.Application.Features.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Features.UsersDbAccesses;
using Demo.DbRefreshManager.Application.Mappings.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Core.Results;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshing;

/// <summary>
/// Остановка ручной перезаливки БД.
/// </summary>
public interface IStopManualRefreshCommandHandler
    : IAsyncHandler<Result<DbRefreshJobDto>, StopManualRefresh.Command>;

public static class StopManualRefresh
{
    public record struct Command(int JobId);

    internal class CommandHandler(
        ICheckCurrentUserDbAccessQueryHandler checkUserHasAccess,
        ISetManualRefreshCanceledCommandHandler setManualRefreshCanceled,
        IGetDbRefreshJobByIdQueryHandler getJobById)
        : IStopManualRefreshCommandHandler
    {
        public async Task<Result<DbRefreshJobDto>> HandleAsync(Command cmd, CancellationToken ct)
        {
            var userHasAccess = await checkUserHasAccess.HandleAsync(new(cmd.JobId), ct);

            if (userHasAccess.IsFailure)
                return userHasAccess.Error;

            await setManualRefreshCanceled.HandleAsync(new(cmd.JobId), ct);

            var updatedJob = await getJobById.HandleAsync(cmd.JobId, ct);
            var dto = updatedJob.ToDto();

            return dto;
        }
    }
}
