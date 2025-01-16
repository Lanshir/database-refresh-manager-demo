using Demo.DbRefreshManager.WebApi.Infrastructure.Helpers.Abstract;
using System.Security.Claims;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.Helpers.Concrete;

/// <inheritdoc cref="IUserIdentityHelper" />
public class UserIdentityHelper(IHttpContextAccessor httpContext) : IUserIdentityHelper
{
    /// <summary>
    /// ClaimsPrincipal авторизованного пользователя.
    /// </summary>
    private readonly ClaimsPrincipal? _userPrincipal = httpContext.HttpContext?.User;

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
    {
        int userId = int.Parse(
            _userPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? "0");

        return userId;
    }

    public string GetUserFullName()
        => _userPrincipal?.FindFirst(_fullNameKey)?.Value ?? "";

    public string GetUserLogin()
        => _userPrincipal?.Identity?.Name ?? "";

    public List<string> GetRoles()
    {
        var roles = _userPrincipal?
            .FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        return roles ?? new List<string>(0);
    }

    public bool IsAuthenticated()
        => _userPrincipal?.Identity?.IsAuthenticated ?? false;
}
