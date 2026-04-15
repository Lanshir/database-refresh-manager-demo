using Demo.DbRefreshManager.Domain.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.WebApi.Models.DbRefreshJobs;
using System.Linq.Expressions;

namespace Demo.DbRefreshManager.WebApi.Mappings.DbRefreshJobs;

public static class DbRefreshLogMappings
{
    extension(DbRefreshLog src)
    {
        /// <summary>
        /// Expression проекции модели записи лога перезаливки БД в dto.
        /// </summary>
        public static Expression<Func<DbRefreshLog, DbRefreshLogDto>> ToDtoProjectionExpression => log => new()
        {
            DbRefreshJobId = log.DbRefreshJobId,
            DbName = log.DbRefreshJob != null ? log.DbRefreshJob.DbName : "",
            RefreshStartDate = log.RefreshStartDate,
            RefreshEndDate = log.RefreshEndDate,
            Code = log.Code,
            Result = log.Result,
            Error = log.Error,
            ExecutedScript = log.ExecutedScript,
            Initiator = log.Initiator,
            GroupCssColor = log.DbRefreshJob != null && log.DbRefreshJob.Group != null
                ? log.DbRefreshJob.Group.CssColor : ""
        };

        private static Func<DbRefreshLog, DbRefreshLogDto> ToDtoFunction => DbRefreshLog.ToDtoProjectionExpression.Compile();

        /// <summary>
        /// Конвертация модели записи лога перезаливки БД в dto.
        /// </summary>
        public DbRefreshLogDto ToDto() => DbRefreshLog.ToDtoFunction(src);
    }
}
