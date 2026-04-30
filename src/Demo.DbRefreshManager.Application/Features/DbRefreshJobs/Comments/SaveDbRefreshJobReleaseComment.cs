using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs.Comments;

/// <summary>
/// Установка релизного комментария задачи на перезаливку БД.
/// </summary>
public interface ISaveDbRefreshJobReleaseCommentCommandHandler
    : IAsyncHandler<bool, SaveDbRefreshJobReleaseComment.Command>;

public class SaveDbRefreshJobReleaseComment
{
    public record struct Command(int JobId, string? Comment);
}
