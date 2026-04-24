using Demo.DbRefreshManager.Application.Features.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Features.UsersDbAccesses;
using Demo.DbRefreshManager.Application.Mappings.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Services;
using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Core.Results;

namespace Demo.DbRefreshManager.Application.Features.DbRefresh;

/// <summary>
/// Команда запуска ручной перезаливки.
/// </summary>
public interface IStartManualRefreshCommandHandler
    : IAsyncHandler<Result<DbRefreshJobDto>, StartManualRefreshCommand.Dto>;

public static class StartManualRefreshCommand
{
    public record struct Dto(int JobId, int DelayMinutes, string? Comment);

    public class Handler(
        IUserIdentityProvider userIdentity,
        ICheckCurrentUserDbAccessQueryHandler checkUserHasAccessCmd,
        ISetManualRefreshStartedCommandHandler setManualRefreshStartedCmd,
        IGetDbRefreshJobByIdQueryHandler getJobByIdQuery)
        : IStartManualRefreshCommandHandler
    {
        public async Task<Result<DbRefreshJobDto>> HandleAsync(Dto cmd, CancellationToken ct)
        {
            var jobId = cmd.JobId;
            var comment = cmd.Comment;
            var initiator = userIdentity.GetUserLogin();
            var delayMinutes = cmd.DelayMinutes > 0 ? cmd.DelayMinutes : 1;
            var refreshDate = DateTime.UtcNow.AddMinutes(delayMinutes);

            var userHasAccess = await checkUserHasAccessCmd.HandleAsync(new(jobId), ct);

            if (userHasAccess.IsFailure)
                return userHasAccess.Error;

            await setManualRefreshStartedCmd.HandleAsync(
                new(jobId, refreshDate, initiator, comment), ct);

            var updatedJob = await getJobByIdQuery.HandleAsync(jobId, ct);
            var dto = updatedJob.ToDto();

            return dto;
        }
    }
}
