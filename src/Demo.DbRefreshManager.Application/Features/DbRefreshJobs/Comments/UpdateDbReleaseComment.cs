using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs.Comments;

/// <summary>
/// Обновление релизного комментария БД.
/// </summary>
public interface IUpdateDbReleaseCommentHandler
    : IAsyncHandler<bool, UpdateDbReleaseComment.Command>;

public class UpdateDbReleaseComment
{
    public record struct Command(int JobId, string? Comment);
}
