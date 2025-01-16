using AutoMapper;
using Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.WebApi.Models.DbRefreshJobs;

namespace Demo.DbRefreshManager.WebApi.Mappings.DbRefreshJobs;

/// <summary>
/// Профиль конвертации доменной модели группы БД в dto.
/// </summary>
public class DbGroupToDtoMapping : Profile
{
    /// <inheritdoc cref="DbGroupToDtoMapping" />
    public DbGroupToDtoMapping()
    {
        CreateMap<DbGroup, DbGroupDto>()
            .ReverseMap();
    }
}
