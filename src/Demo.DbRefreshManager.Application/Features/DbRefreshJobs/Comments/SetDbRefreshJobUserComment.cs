using Demo.DbRefreshManager.Application.Features.DbAccesses;
using Demo.DbRefreshManager.Application.Mappings.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Core.Results;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs.Comments;

public interface ISetDbRefreshJobUserCommentHandler
    : IAsyncHandler<Result<DbRefreshJobDto>, SetDbRefreshJobUserComment.Command>;

public class SetDbRefreshJobUserComment
{
    public record struct Command(int JobId, string? Comment);

    internal class Handler(
        ICheckCurrentUserDbAccessHandler checkUserHasAccess,
        IUpdateDbRefreshJobUserCommentHandler updateDbRefreshJobUserComment,
        IGetDbRefreshJobByIdHandler getDbRefreshJobById
        ) : ISetDbRefreshJobUserCommentHandler
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
