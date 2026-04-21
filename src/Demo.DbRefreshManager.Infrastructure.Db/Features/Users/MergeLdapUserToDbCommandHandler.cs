using Demo.DbRefreshManager.Application.Features.Users;
using Demo.DbRefreshManager.Domain.Entities.ActiveDirectory;
using Demo.DbRefreshManager.Domain.Entities.Users;
using Demo.DbRefreshManager.Domain.Mappings;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.Users;

internal class MergeLdapUserToDbCommandHandler(
    IDbContextFactory<AppDbContext> contextFactory
    ) : IMergeLdapUserToDbCommandHandler
{
    public async Task<User> HandleAsync(LdapUser ldapUser, CancellationToken ct)
    {
        using var ctx = await contextFactory.CreateDbContextAsync(ct);
        ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

        var dbUser = await ctx.Set<User>()
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.LdapLogin.ToUpper() == ldapUser.Login.ToUpper(), ct);

        var ldapUserRoles = await ctx.Set<UserRole>()
            .Where(r => r.IsActive
                && ldapUser.Groups.Contains(r.LdapGroup!))
            .ToListAsync(ct);

        // Создание пользователя, если нет существующего.
        if (dbUser == null)
        {
            dbUser = ldapUser.ToDomainUser();
            dbUser.Roles = ldapUserRoles;

            ctx.Add(dbUser);

            await ctx.SaveChangesAsync(ct);

            return dbUser;
        }

        // Обновление пользователя, если данные ldap изменились.
        if (dbUser.LdapChangeDate != ldapUser.WhenChanged)
        {
            dbUser = ldapUser.ToDomainUser().MergeTo(dbUser);
            dbUser.ModifyDate = DateTime.UtcNow;

            // Синхронизация привязок ролей пользователя с ldap.
            dbUser.Roles.RemoveAll(ur => !ldapUserRoles.Any(r => r.Id == ur.Id));
            dbUser.Roles.AddRange(ldapUserRoles.Where(r => !dbUser.Roles.Any(ur => ur.Id == r.Id)));

            await ctx.SaveChangesAsync(ct);
        }

        return dbUser;
    }
}