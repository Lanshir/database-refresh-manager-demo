using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs.Comments;

/// <summary>
/// Установка релизного комментария задачи на перезаливку БД.
/// </summary>
public interface IUpdateDbRefreshJobReleaseCommentHandler
    : IAsyncHandler<bool, UpdateDbRefreshJobReleaseComment.Command>;

public class UpdateDbRefreshJobReleaseComment
{
    public record struct Command(int JobId, string? Comment);
}
