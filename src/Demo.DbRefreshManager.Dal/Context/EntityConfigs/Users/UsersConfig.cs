using Demo.DbRefreshManager.Dal.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.DbRefreshManager.Dal.Context.EntityConfigs.Users;

/// <summary>
/// Конфигурация контекста пользователей.
/// </summary>
internal class UsersConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users").HasKey(u => u.Id);

        builder.Property(u => u.Id).HasColumnName("id");
        builder.Property(u => u.FirstName).HasColumnName("first_name");
        builder.Property(u => u.Patronymic).HasColumnName("patronymic");
        builder.Property(u => u.LastName).HasColumnName("last_name");
        builder.Property(u => u.Email).HasColumnName("email");
        builder.Property(u => u.LdapLogin).HasColumnName("ldap_login");
        builder.Property(u => u.LdapDn).HasColumnName("ldap_dn");
        builder.Property(u => u.LdapChangeDate).HasColumnName("ldap_change_date");

        builder.Property(u => u.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("datetime()")
            .ValueGeneratedOnAddOrUpdate();

        builder.Property(u => u.ModifyDate)
            .HasColumnName("modify_date")
            .HasDefaultValueSql("datetime()")
            .ValueGeneratedOnAdd();

        builder.HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<UserRoleBind>();
    }
}
