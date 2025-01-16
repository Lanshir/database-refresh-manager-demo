using Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.DbRefreshManager.Dal.Context.EntityConfigs.DbRefreshJobs;

/// <summary>
/// Конфигурация связок группа БД - роль пользователя.
/// </summary>
public class DbGroupRoleBindsConfig : IEntityTypeConfiguration<DbGroupRoleBind>
{
    public void Configure(EntityTypeBuilder<DbGroupRoleBind> builder)
    {
        builder.ToTable("db_groups_roles")
            .HasKey(e => new { e.GroupId, e.RoleId });

        builder.Property(b => b.GroupId).HasColumnName("db_group_id");
        builder.Property(b => b.RoleId).HasColumnName("role_id");

        builder.HasOne(b => b.Group).WithMany();
        builder.HasOne(b => b.Role).WithMany();
    }
}
