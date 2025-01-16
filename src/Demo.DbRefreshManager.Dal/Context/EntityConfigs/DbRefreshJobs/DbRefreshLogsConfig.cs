using Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.DbRefreshManager.Dal.Context.EntityConfigs.DbRefreshJobs;

/// <summary>
/// Конфигурация логов переаливки БД.
/// </summary>
internal class DbRefreshLogsConfig : IEntityTypeConfiguration<DbRefreshLog>
{
    public void Configure(EntityTypeBuilder<DbRefreshLog> builder)
    {
        builder.ToTable("db_refresh_logs")
            .HasKey(l => new { l.DbRefreshJobId, l.RefreshStartDate });

        builder.Property(l => l.DbRefreshJobId).HasColumnName("db_refresh_job_id");
        builder.Property(l => l.RefreshStartDate).HasColumnName("refresh_start_date");
        builder.Property(l => l.RefreshEndDate).HasColumnName("refresh_end_date");
        builder.Property(l => l.Code).HasColumnName("code");
        builder.Property(l => l.Result).HasColumnName("result_text");
        builder.Property(l => l.Error).HasColumnName("error");
        builder.Property(l => l.ExecutedScript).HasColumnName("executed_script");
        builder.Property(l => l.Initiator).HasColumnName("initiator");
    }
}
