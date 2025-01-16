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
    public async Task<User> MergeLdapUser(User ldapUserData)
    {
        var ctx = await ContextFactory.CreateDbContextAsync();
        var ctx2 = await ContextFactory.CreateDbContextAsync();

        var userTask = ctx.Set<User>()
            .Include(u => u.RolesBinds)
            .Include(u => u.Roles)
            .AsTracking()
            .FirstOrDefaultAsync(u =>
                u.LdapLogin.ToUpper() == ldapUserData.LdapLogin.ToUpper());

        var dbLdapRolesTask = ctx2.Set<UserRole>()
            .Where(r => r.IsActive
                && ldapUserData.Roles
                    .Select(r1 => r1.LdapGroup)
                    .Contains(r.LdapGroup))
            .ToListAsync();

        var user = await userTask;
        var dbLdapRoles = await dbLdapRolesTask;

        if (user == null)
        {
            user = mapper.Map(ldapUserData, new User());
            user.Roles = dbLdapRoles;

            // Привязка ролей к основному контексту для избежания вставки вместе с User.
            ctx.AttachRange(dbLdapRoles);
            ctx.Add(user);

            await ctx.SaveChangesAsync();

            return user;
        }

        // Обновление данных пользователей если изменился ldapChangeDate или список групп.
        var userRoleIds = user.Roles.Select(r => r.Id).Order().ToList();
        var ldapRoleIds = dbLdapRoles.Select(r => r.Id).Order().ToList();

        if (user.LdapChangeDate != ldapUserData.LdapChangeDate
            || !userRoleIds.SequenceEqual(ldapRoleIds))
        {
            user = mapper.Map(ldapUserData, user);
            user.ModifyDate = DateTime.UtcNow;

            if (!userRoleIds.SequenceEqual(ldapRoleIds))
            {
                // Удалить роли пользователя, которых нет в ldap.
                ctx.Set<UserRoleBind>().RemoveRange(user.RolesBinds
                    .Where(b => !ldapRoleIds.Contains(b.RoleId)));

                // Добавить роли из ldap, которых нет у пользователя.
                ctx.Set<UserRoleBind>().AddRange(ldapRoleIds
                    .Where(roleId => !userRoleIds.Contains(roleId))
                    .Select(roleId => new UserRoleBind { UserId = user.Id, RoleId = roleId }));
            }

            // Отправка изменений в БД.
            await ctx.SaveChangesAsync();

            user.Roles = dbLdapRoles;
        }

        return user;
    }
}
