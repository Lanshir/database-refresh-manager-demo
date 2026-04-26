using Demo.DbRefreshManager.Application.Features.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Features.UsersDbAccesses;
using Demo.DbRefreshManager.Application.Mappings.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Core.Results;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshing;

/// <summary>
/// Команда оствновки ручной перезаливки БД.
/// </summary>
public interface IStopManualRefreshCommandHandler
    : IAsyncHandler<Result<DbRefreshJobDto>, StopManualRefreshCommand.Dto>;

public static class StopManualRefreshCommand
{
    public record struct Dto(int JobId);

    public class Handler(
        ICheckCurrentUserDbAccessQueryHandler checkUserHasAccess,
        ISetManualRefreshCanceledCommandHandler setManualRefreshCanceledCmd,
        IGetDbRefreshJobByIdQueryHandler getJobByIdQuery)
        : IStopManualRefreshCommandHandler
    {
        public async Task<Result<DbRefreshJobDto>> HandleAsync(Dto cmd, CancellationToken ct)
        {
            var userHasAccess = await checkUserHasAccess.HandleAsync(new(cmd.JobId), ct);

            if (userHasAccess.IsFailure)
                return userHasAccess.Error;

            await setManualRefreshCanceledCmd.HandleAsync(new(cmd.JobId), ct);

            var updatedJob = await getJobByIdQuery.HandleAsync(cmd.JobId, ct);
            var dto = updatedJob.ToDto();

            return dto;
        }
    }
}
