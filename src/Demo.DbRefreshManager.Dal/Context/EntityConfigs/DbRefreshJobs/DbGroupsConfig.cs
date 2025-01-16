using Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.DbRefreshManager.Dal.Context.EntityConfigs.DbRefreshJobs;

/// <summary>
/// Конфигурация групп БД.
/// </summary>
internal class DbGroupsConfig : IEntityTypeConfiguration<DbGroup>
{
    public void Configure(EntityTypeBuilder<DbGroup> builder)
    {
        builder.ToTable("db_groups").HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(l => l.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("datetime()")
            .ValueGeneratedOnAddOrUpdate();

        builder.Property(l => l.SortOrder).HasColumnName("sort_order");
        builder.Property(l => l.CssColor).HasColumnName("css_color");
        builder.Property(l => l.Description).HasColumnName("description");

        builder.Property(l => l.IsVisible)
            .HasColumnName("is_visible")
            .HasDefaultValue(true);

        builder.HasMany(g => g.AccessRoles)
            .WithMany()
            .UsingEntity<DbGroupRoleBind>();
    }
}
