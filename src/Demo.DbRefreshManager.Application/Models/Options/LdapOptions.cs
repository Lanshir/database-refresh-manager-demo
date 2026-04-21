namespace Demo.DbRefreshManager.Application.Models.Options;

public class LdapOptions
{
    /// <summary>
    /// Хост домена.
    /// </summary>
    public string Host { get; init; } = string.Empty;

    /// <summary>
    /// Базовые пути для поиска юзеров в домене.
    /// </summary>
    public List<string> UserSearchDnBases { get; init; } = [];
}
