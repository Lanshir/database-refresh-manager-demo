using Demo.DbRefreshManager.Domain.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.WebApi.Models.DbRefreshJobs;
using System.Linq.Expressions;

namespace Demo.DbRefreshManager.WebApi.Mappings.DbRefreshJobs;

public static class DbGroupMappings
{
    extension(DbGroup src)
    {
        /// <summary>
        /// Expression проекции модели группы БД в dto.
        /// </summary>
        public static Expression<Func<DbGroup, DbGroupDto>> ToDtoProjectionExpression => grp => new()
        {
            Id = grp.Id,
            SortOrder = grp.SortOrder,
            Description = grp.Description,
            CssColor = grp.CssColor
        };

        private static Func<DbGroup, DbGroupDto> ToDtoFunction
            => DbGroup.ToDtoProjectionExpression.Compile();

        /// <summary>
        /// Конвертация модели группы БД в dto.
        /// </summary>
        public DbGroupDto ToDto() => DbGroup.ToDtoFunction(src);
    }
}
