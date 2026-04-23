using Demo.DbRefreshManager.Application.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Core.Extensions;
using Demo.DbRefreshManager.Domain.Entities.DbRefreshJobs;
using System.Linq.Expressions;

namespace Demo.DbRefreshManager.Application.Mappings.DbRefreshJobs;

public static class DbRefreshJobMappings
{
    extension(DbRefreshJob src)
    {
        /// <summary>
        /// Expression проекции модели задачи на перезаливку БД в dto.
        /// </summary>
        public static Expression<Func<DbRefreshJob, DbRefreshJobDto>> ToDtoProjectionExpression => job => new()
        {
            Id = job.Id,
            DbName = job.DbName,
            InProgress = job.InProgress,
            ScheduleIsActive = job.ScheduleIsActive,
            ManualRefreshDate = job.ManualRefreshDate,
            ScheduleRefreshDate = job.ScheduleRefreshTime.TimeToTodayDateUtc() > DateTime.UtcNow
                ? job.ScheduleRefreshTime.TimeToTodayDateUtc()
                : job.ScheduleRefreshTime.TimeToTodayDateUtc().AddDays(1),
            LastRefreshDate = job.LastRefreshDate,
            ScheduleChangeUser = job.ScheduleChangeUser == null ? null : job.ScheduleChangeUser.LdapLogin,
            ScheduleChangeDate = job.ScheduleChangeDate,
            ReleaseComment = job.ReleaseComment,
            UserComment = job.UserComment,
            GroupSortOrder = job.Group == null ? default : job.Group.SortOrder,
            GroupCssColor = job.Group == null ? "" : job.Group.CssColor,
            GroupAccessRoles = job.Group == null ? new(0) : job.Group.AccessRoles.Select(r => r.Name).ToList()
        };

        private static Func<DbRefreshJob, DbRefreshJobDto> ToDtoFunction =>
            DbRefreshJob.ToDtoProjectionExpression.Compile();

        /// <summary>
        /// Конвертация модели задачи на перезаливку БД в dto.
        /// </summary>
        public DbRefreshJobDto ToDto() => DbRefreshJob.ToDtoFunction(src);
    }
}
