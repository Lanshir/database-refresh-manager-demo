using Demo.DbRefreshManager.Application.Services;
using System.Security.Claims;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.Services;

/// <inheritdoc cref="IUserIdentityProvider" />
public class UserIdentityProvider(IHttpContextAccessor httpContextAccessor) : IUserIdentityProvider
{
    /// <summary>
    /// ClaimsPrincipal авторизованного пользователя.
    /// </summary>
    private readonly ClaimsPrincipal _userPrincipal = httpContextAccessor.HttpContext?.User
        ?? throw new Exception("Identity provider was unable to access http context.");

    private readonly string _fullNameKey = "FullName";

    public List<Claim> CreateClaimsList(
        int userId,
        string login,
        string fullName,
        List<string> roles)
    {
        var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, userId.ToString()),
                new (ClaimTypes.Name, login),
                new (_fullNameKey, fullName)
            };

        roles?.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));

        return claims;
    }

    public int GetUserId()
        => int.Parse(
            _userPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? "0");

    public string GetUserFullName()
        => _userPrincipal.FindFirst(_fullNameKey)?.Value ?? "";

    public string GetUserLogin()
        => _userPrincipal.Identity?.Name ?? "";

    public List<string> GetRoles()
    {
        var roles = _userPrincipal
            .FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        return roles;
    }

    public bool IsAuthenticated()
        => _userPrincipal.Identity?.IsAuthenticated ?? false;
}
