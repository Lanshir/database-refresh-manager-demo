using Demo.DbRefreshManager.Application.Features.DbAccesses;
using Demo.DbRefreshManager.Application.Mappings.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Core.Results;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs.Comments;

/// <summary>
/// Изменить пользовательский комментарий к БД.
/// </summary>
public interface ISetDbUserCommentHandler
    : IAsyncHandler<Result<DbRefreshJobDto>, SetDbUserComment.Command>;

public class SetDbUserComment
{
    public record struct Command(int JobId, string? Comment);

    internal class Handler(
        ICheckCurrentUserDbAccessHandler checkUserHasAccess,
        IUpdateDbUserCommentHandler updateDbRefreshJobUserComment,
        IGetDbRefreshJobByIdHandler getDbRefreshJobById
        ) : ISetDbUserCommentHandler
    {
        public async Task<Result<DbRefreshJobDto>> HandleAsync(Command cmd, CancellationToken ct)
        {
            var userHasAccess = await checkUserHasAccess.HandleAsync(new(cmd.JobId), ct);

            if (userHasAccess.IsFailure)
                return userHasAccess.Error;

            await updateDbRefreshJobUserComment.HandleAsync(new(cmd.JobId, cmd.Comment), ct);

            var updatedJob = await getDbRefreshJobById.HandleAsync(cmd.JobId, ct);
            var dto = updatedJob.ToDto();

            return dto;
        }
    }
}
