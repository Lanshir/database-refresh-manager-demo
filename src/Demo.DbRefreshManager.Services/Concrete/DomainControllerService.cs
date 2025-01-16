using Demo.DbRefreshManager.Services.Abstract;
using Demo.DbRefreshManager.Services.Infrastructure.Constants;
using Demo.DbRefreshManager.Services.Models.ActiveDirectory;
using Novell.Directory.Ldap;
using System.Diagnostics;
using System.Globalization;

namespace Demo.DbRefreshManager.Services.Concrete;

/// <summary>
/// Провайдер авторизации через контроллер домена (Windows Active Directory).
/// </summary>
public class DomainControllerService : IDomainControllerService
{
    /// <summary>
    /// Подключение к домену.
    /// </summary>
    private LdapConnection? _connection;

    public bool IsAuthorized { get; private set; }

    public bool Connect(
        string domainHost,
        int retryCount = 0,
        int retryDelayMs = 1000)
    {
        if (_connection?.Connected ?? false)
        {
            return true;
        }

        _connection = new LdapConnection() { SecureSocketLayer = false };

        while (retryCount >= 0)
        {
            try
            {
                _connection.Connect(domainHost, LdapConnection.DefaultPort);

                if (_connection.Connected)
                {
                    return true;
                }
            }
            catch
            {
                if (retryCount == 0)
                {
                    throw;
                }
            }

            retryCount--;

            Thread.Sleep(retryDelayMs);
        }

        return false;
    }

    public bool Authorize(string login, string password)
    {
        try
        {
            var userDomainName = $"{login}@{_connection?.Host}";

            _connection?.Bind(userDomainName, password);

            IsAuthorized = _connection?.Bound ?? false;

            return _connection?.Bound ?? false;
        }
        catch (Exception exc)
        {
            Debug.WriteLine(exc);

            IsAuthorized = false;

            return false;
        }
    }

    public LdapUser? GetUserData(string login, IEnumerable<string> searchDnBases)
    {
        try
        {
            LdapEntry? userEntry = null;

            // Поиск записи пользователя по указанным путям до первого совпадения.
            foreach (var searchBase in searchDnBases)
            {
                userEntry = _connection?.Search(
                    @base: searchBase,
                    scope: LdapConnection.ScopeSub,
                    filter: $"({LdapAttributes.Login}={login})",
                    attrs: null,
                    typesOnly: false
                ).ToList().FirstOrDefault();

                if (userEntry != null)
                {
                    break;
                }
            }

            if (userEntry == null)
            {
                return null;
            }

            const string unknown = "Unknown";

            string email = "";
            string fullName = $"{unknown} {unknown} {unknown}";
            string firstName = unknown;
            string lastName = unknown;
            string patronymic = unknown;
            var groups = new List<string>();

            // LDAP кидает Exception если атрибут не найден.
            try { fullName = userEntry.GetAttribute(LdapAttributes.FullName)?.StringValue ?? ""; }
            catch (Exception exc) { Debug.WriteLine(exc); }
            try { firstName = userEntry.GetAttribute(LdapAttributes.FirstName)?.StringValue ?? ""; }
            catch (Exception exc) { Debug.WriteLine(exc); }
            try { lastName = userEntry.GetAttribute(LdapAttributes.LastName)?.StringValue ?? ""; }
            catch (Exception exc) { Debug.WriteLine(exc); }
            try { patronymic = fullName.Replace(firstName, "").Replace(lastName, "").Trim(); }
            catch (Exception exc) { Debug.WriteLine(exc); }
            try { email = userEntry.GetAttribute(LdapAttributes.Email)?.StringValue ?? ""; }
            catch (Exception exc) { Debug.WriteLine(exc); }

            try
            {
                groups = userEntry.GetAttribute(LdapAttributes.GroupsList)
                    ?.StringValueArray.Select(grpDn => grpDn
                        .Split(",")
                        .Where(s => s.StartsWith("CN="))
                        .FirstOrDefault()
                        ?.Replace("CN=", "") ?? "")
                    .ToList();
            }
            catch (Exception exc) { Debug.WriteLine(exc); }

            var user = new LdapUser
            {
                Dn = userEntry.Dn,
                Login = userEntry.GetAttribute(LdapAttributes.Login).StringValue,
                Email = email,
                FirstName = firstName,
                Patronymic = patronymic,
                LastName = lastName,
                FullName = fullName,
                WhenChanged = DateTime.ParseExact(
                    userEntry.GetAttribute(LdapAttributes.WhenChanged).StringValue,
                    "yyyyMMddHHmmss.fK",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal),
                Groups = groups ?? []
            };

            // Опциональные параетры.
            try { user.Company = userEntry.GetAttribute(LdapAttributes.Company).StringValue; }
            catch (Exception exc) { Debug.WriteLine(exc); user.Company = null; }
            try { user.Department = userEntry.GetAttribute(LdapAttributes.Department).StringValue; }
            catch (Exception exc) { Debug.WriteLine(exc); user.Department = null; }
            try { user.Position = userEntry.GetAttribute(LdapAttributes.Position).StringValue; }
            catch (Exception exc) { Debug.WriteLine(exc); user.Position = null; }

            return user;
        }
        catch (Exception exc)
        {
            Debug.WriteLine(exc);
            return null;
        }
    }

    public void Dispose()
    {
        if (_connection?.Connected ?? false)
        {
            _connection.Disconnect();
        }

        _connection?.Dispose();

        GC.SuppressFinalize(this);
    }
}
