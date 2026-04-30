using Demo.DbRefreshManager.Application.Mappings.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Core.Results;
using Demo.DbRefreshManager.Domain.Errors;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs.Comments;

public interface ISetDbRefreshJobReleaseCommentCommandHandler
    : IAsyncHandler<Result<DbRefreshJobDto>, SetDbRefreshJobReleaseComment.Command>;

public class SetDbRefreshJobReleaseComment
{
    /// <param name="DbName">Название базы.</param>
    /// <param name="Comment">Комментарий.</param>
    /// <param name="IsAppend">Добавить комментарий к предыдущему.</param>
    public record struct Command(
        string DbName,
        string? Comment,
        bool IsAppend = false);

    internal class Handler(
        IFindDbRefreshJobQueryHandler findDbRefreshJob,
        ISaveDbRefreshJobReleaseCommentCommandHandler saveDbRefreshJobReleaseComment)
        : ISetDbRefreshJobReleaseCommentCommandHandler
    {
        public async Task<Result<DbRefreshJobDto>> HandleAsync(Command cmd, CancellationToken ct)
        {
            var (dbName, comment, isAppend) = cmd;
            var job = await findDbRefreshJob.HandleAsync(new(DbName: dbName), ct);

            if (job is null)
                return DbRefreshJobErrors.NotFound with
                {
                    Message = $"БД с названием {cmd.DbName} не найдена"
                };

            var newReleaseComment = isAppend ? (job.ReleaseComment + comment).Trim('\n') : comment;

            await saveDbRefreshJobReleaseComment.HandleAsync(new(job.Id, newReleaseComment), ct);

            job.ReleaseComment = newReleaseComment;

            var dto = job.ToDto();

            return dto;
        }
    }
}
