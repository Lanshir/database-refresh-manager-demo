using Demo.DbRefreshManager.Dal.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.DbRefreshManager.Dal.Context.EntityConfigs.Users;

/// <summary>
/// Конфигурация ролей пользователей.
/// </summary>
internal class UserRolesConfig : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("roles").HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(r => r.Name).HasColumnName("name");
        builder.Property(r => r.Description).HasColumnName("description");
        builder.Property(r => r.LdapGroup).HasColumnName("ldap_group");

        builder.Property(r => r.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(r => r.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("datetime()")
            .ValueGeneratedOnAddOrUpdate();
    }
}
