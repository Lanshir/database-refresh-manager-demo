using Demo.DbRefreshManager.Dal.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.DbRefreshManager.Dal.Context.EntityConfigs.Users;

/// <summary>
/// Конфигурация связок пользователь-роль.
/// </summary>
internal class UserRoleBindsConfig : IEntityTypeConfiguration<UserRoleBind>
{
    public void Configure(EntityTypeBuilder<UserRoleBind> builder)
    {
        builder.ToTable("users_roles")
            .HasKey(e => new { e.UserId, e.RoleId });

        builder.Property(b => b.UserId).HasColumnName("user_id");
        builder.Property(b => b.RoleId).HasColumnName("role_id");

        builder.HasOne(b => b.User).WithMany();
        builder.HasOne(b => b.Role).WithMany();
    }
}
