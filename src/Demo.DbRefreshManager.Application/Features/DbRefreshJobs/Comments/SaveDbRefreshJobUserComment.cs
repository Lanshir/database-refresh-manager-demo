using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs.Comments;

/// <summary>
/// Сохранение пользовательского комментария к задаче на перезаливку.
/// </summary>
public interface ISaveDbRefreshJobUserCommentCommandHandler
    : IAsyncHandler<bool, SaveDbRefreshJobUserComment.Command>;

public class SaveDbRefreshJobUserComment
{
    public record struct Command(int JobId, string? Comment);
}
