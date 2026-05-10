using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs.Comments;

/// <summary>
/// Обновление пользовательского комментария к БД.
/// </summary>
public interface IUpdateDbUserCommentHandler
    : IAsyncHandler<bool, UpdateDbUserComment.Command>;

public class UpdateDbUserComment
{
    public record struct Command(int JobId, string? Comment);
}
