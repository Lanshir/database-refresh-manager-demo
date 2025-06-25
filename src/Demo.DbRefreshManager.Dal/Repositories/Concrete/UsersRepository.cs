using Demo.DbRefreshManager.Common.Converters.Abstract;
using Demo.DbRefreshManager.Dal.Context;
using Demo.DbRefreshManager.Dal.Entities.Users;
using Demo.DbRefreshManager.Dal.Repositories.Abstract;
using Demo.DbRefreshManager.Dal.Repositories.Concrete.Base;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Dal.Repositories.Concrete;

/// <inheritdoc cref="IUsersRepository" />
internal class UsersRepository(
    IDbContextFactory<AppDbContext> contextFactory,
    ITypeMapper mapper
    ) : BaseRepository<User>(contextFactory), IUsersRepository
{
    public async Task<User> MergeLdapUser(User ldapUser)
    {
        using var ctx = await ContextFactory.CreateDbContextAsync();
        ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

        var user = await ctx.Set<User>()
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.LdapLogin.ToUpper() == ldapUser.LdapLogin.ToUpper());

        var ldapUserRoles = await ctx.Set<UserRole>()
            .Where(r => r.IsActive
                && ldapUser.Roles
                    .Select(r1 => r1.LdapGroup)
                    .Contains(r.LdapGroup))
            .ToListAsync();

        if (user == null)
        {
            user = mapper.Map(ldapUser, new User());
            user.Roles = ldapUserRoles;

            ctx.Add(user);

            await ctx.SaveChangesAsync();

            return user;
        }

        // Обновление пользователя если данные ldap изменились.
        if (user.LdapChangeDate != ldapUser.LdapChangeDate)
        {
            user = mapper.Map(ldapUser, user);
            user.ModifyDate = DateTime.UtcNow;

            // Синхронизация привязок ролей пользователя с ldap.
            user.Roles.RemoveAll(ur => !ldapUserRoles.Any(r => r.Id == ur.Id));
            user.Roles.AddRange(ldapUserRoles.Where(r => !user.Roles.Any(ur => ur.Id == r.Id)));

            await ctx.SaveChangesAsync();
        }

        return user;
    }
}
