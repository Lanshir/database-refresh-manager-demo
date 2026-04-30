using Demo.DbRefreshManager.Application.Features.DbAccesses;
using Demo.DbRefreshManager.Application.Mappings.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Services;
using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Core.Results;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs.ScheduledRefresh;

/// <summary>
/// Переключение активности перезаливки по расписанию.
/// </summary>
public interface ISetScheduledRefreshAvtiveCommandHandler
    : IAsyncHandler<Result<DbRefreshJobDto>, SetScheduledRefreshActive.Command>;

public class SetScheduledRefreshActive
{
    public record struct Command(int JobId, bool IsActive);

    internal class Handler(
        IUserIdentityProvider userIdentity,
        ICheckCurrentUserDbAccessQueryHandler checkUserHasAccess,
        ISaveScheduledRefreshActiveCommandHandler saveScheduledRefreshActive,
        IGetDbRefreshJobByIdQueryHandler getDbRefreshJobById)
        : ISetScheduledRefreshAvtiveCommandHandler
    {
        public async Task<Result<DbRefreshJobDto>> HandleAsync(Command cmd, CancellationToken ct)
        {
            var changeUserId = userIdentity.GetUserId();
            var userHasAccess = await checkUserHasAccess.HandleAsync(new(cmd.JobId), ct);

            if (userHasAccess.IsFailure)
                return userHasAccess.Error;

            await saveScheduledRefreshActive.HandleAsync(new(cmd.JobId, changeUserId, cmd.IsActive), ct);

            var updatedJob = await getDbRefreshJobById.HandleAsync(cmd.JobId, ct);
            var dto = updatedJob.ToDto();

            return dto;
        }
    }
}
