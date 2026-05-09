using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs.Comments;

/// <summary>
/// Сохранение пользовательского комментария к задаче на перезаливку.
/// </summary>
public interface IUpdateDbRefreshJobUserCommentHandler
    : IAsyncHandler<bool, UpdateDbRefreshJobUserComment.Command>;

public class UpdateDbRefreshJobUserComment
{
    public record struct Command(int JobId, string? Comment);
}
