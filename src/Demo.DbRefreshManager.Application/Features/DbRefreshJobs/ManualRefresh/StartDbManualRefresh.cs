using Demo.DbRefreshManager.Application.Features.DbAccesses;
using Demo.DbRefreshManager.Application.Mappings.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Services;
using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Core.Results;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs.ManualRefresh;

/// <summary>
/// Запуск ручной перезаливки.
/// </summary>
public interface IStartDbManualRefreshHandler
    : IAsyncHandler<Result<DbRefreshJobDto>, StartDbManualRefresh.Command>;

public static class StartDbManualRefresh
{
    public record struct Command(int JobId, int DelayMinutes, string? Comment);

    internal class Handler(
        IUserIdentityProvider userIdentity,
        ICheckCurrentUserDbAccessHandler checkUserHasAccess,
        IUpdateDbManualRefreshStartedHandler updateDbManualRefreshStarted,
        IGetDbRefreshJobByIdHandler getJobById)
        : IStartDbManualRefreshHandler
    {
        public async Task<Result<DbRefreshJobDto>> HandleAsync(Command cmd, CancellationToken ct)
        {
            var (jobId, _, comment) = cmd;
            var delayMinutes = cmd.DelayMinutes > 0 ? cmd.DelayMinutes : 1;
            var refreshDate = DateTime.UtcNow.AddMinutes(delayMinutes);
            var initiator = userIdentity.GetUserLogin();

            var userHasAccess = await checkUserHasAccess.HandleAsync(new(jobId), ct);

            if (userHasAccess.IsFailure)
                return userHasAccess.Error;

            await updateDbManualRefreshStarted.HandleAsync(
                new(jobId, refreshDate, initiator, comment), ct);

            var updatedJob = await getJobById.HandleAsync(jobId, ct);
            var dto = updatedJob.ToDto();

            return dto;
        }
    }
}
