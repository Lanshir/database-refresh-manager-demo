using Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.DbRefreshManager.Dal.Context.EntityConfigs.DbRefreshJobs;

/// <summary>
/// Конфигурация задач на перезливку.
/// </summary>
internal class DbRefreshJobsConfig : IEntityTypeConfiguration<DbRefreshJob>
{
    public void Configure(EntityTypeBuilder<DbRefreshJob> builder)
    {
        builder.ToTable("db_refresh_jobs").HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(m => m.InProgress)
            .HasColumnName("in_progress")
            .HasDefaultValue(false);

        builder.Property(m => m.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false);

        builder.Property(m => m.ScheduleIsActive)
            .HasColumnName("schedule_is_active")
            .HasDefaultValue(false);

        builder.Property(m => m.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("datetime()")
            .ValueGeneratedOnAddOrUpdate();

        builder.Property(m => m.DbName).HasColumnName("db_name");
        builder.Property(m => m.GroupId).HasColumnName("db_group_id");
        builder.Property(m => m.ManualRefreshDate).HasColumnName("manual_refresh_date");
        builder.Property(m => m.ManualRefreshInitiator).HasColumnName("manual_refresh_initiator");

        builder.Property(m => m.ScheduleRefreshTime)
            .HasColumnName("schedule_refresh_time")
            .ValueGeneratedOnUpdate();

        builder.Property(m => m.SshScript).HasColumnName("ssh_script");

        builder.Property(m => m.LastRefreshDate)
            .HasColumnName("last_refresh_date")
            .HasDefaultValueSql("datetime()")
            .ValueGeneratedOnAdd();

        builder.Property(m => m.ReleaseComment).HasColumnName("release_comment");
        builder.Property(m => m.UserComment).HasColumnName("user_comment");
        builder.Property(m => m.ScheduleChangeDate).HasColumnName("schedule_change_date");
        builder.Property(m => m.ScheduleChangeUserId).HasColumnName("schedule_change_user_id");
    }
}
