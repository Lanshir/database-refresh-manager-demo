using Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.DbRefreshManager.Dal.Context.EntityConfigs.DbRefreshJobs;

/// <summary>
/// Конфигурация персональных доступов к БД.
/// </summary>
public class DbPersonalAccessesConfig : IEntityTypeConfiguration<DbPersonalAccess>
{
    public void Configure(EntityTypeBuilder<DbPersonalAccess> builder)
    {
        builder.ToTable("db_personal_access")
            .HasKey(e => new { e.JobId, e.Login });

        builder.Property(e => e.JobId).HasColumnName("db_refresh_job_id");
        builder.Property(e => e.Login).HasColumnName("user_login");
    }
}
