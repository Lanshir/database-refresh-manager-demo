using Demo.DbRefreshManager.Application.Services;
using Demo.DbRefreshManager.Domain.Models.ActiveDirectory;

namespace Demo.DbRefreshManager.Infrastructure.Services;

internal class DomainControllerServiceFake : IDomainControllerService
{
    public bool IsAuthenticated { get; private set; } = false;

    public bool Connect(
        string domainHost,
        int reconnectCount = 0,
        int reconnectDelayMs = 1000)
    {
        return true;
    }

    public bool Authenticate(string login, string password)
    {
        if (password == "pwd" && _demoUsers.Any(u => u.Login == login))
        {
            IsAuthenticated = true;
            return true;
        }

        return false;
    }

    public LdapUser? GetUserData(string login, IEnumerable<string> searchDnBases)
    {
        return _demoUsers.FirstOrDefault(u => u.Login == login);
    }

    public void Dispose() { }

    private static readonly List<LdapUser> _demoUsers = new()
    {
        {
            new LdapUser
            {
                Dn = "demoMaster",
                Login = "demoMaster",
                Company = "Рога и копыта",
                Department = "Отдел прокрастинации",
                Email = "demo@demo.ru",
                FirstName = "Иван",
                Patronymic = "Иванович",
                LastName = "Иванов",
                FullName = "Иванов Иван Иванович",
                Position = "Мастер",
                WhenChanged = DateTime.UtcNow,
                Groups = ["RefreshManagerMaster"]
            }
        },
        {
            new LdapUser
            {
                Dn = "demoAnalyst",
                Login = "demoAnalyst",
                Company = "Рога и копыта",
                Department = "Отдел прокрастинации",
                Email = "demo@demo.ru",
                FirstName = "Иван",
                Patronymic = "Иванович",
                LastName = "Иванов",
                FullName = "Иванов Иван Иванович",
                Position = "Аналитик",
                WhenChanged = DateTime.UtcNow,
                Groups = ["RefreshManagerAnalyst"]
            }
        },
        {
            new LdapUser
            {
                Dn = "demoSupport",
                Login = "demoSupport",
                Company = "Рога и копыта",
                Department = "Отдел прокрастинации",
                Email = "demo@demo.ru",
                FirstName = "Иван",
                Patronymic = "Иванович",
                LastName = "Иванов",
                FullName = "Иванов Иван Иванович",
                Position = "Тех. поддержка",
                WhenChanged = DateTime.UtcNow,
                Groups = ["RefreshManagerSupport"]
            }
        }
    };
}
