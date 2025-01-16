using AutoMapper;
using Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.WebApi.Models.DbRefreshJobs;

namespace Demo.DbRefreshManager.WebApi.Mappings.DbRefreshJobs;

/// <summary>
/// Профиль конвертации записи лога перезаливки БД в dto.
/// </summary>
public class DbRefreshLogToDtoMapping : Profile
{
    /// <inheritdoc cref="DbRefreshLogToDtoMapping" />
    public DbRefreshLogToDtoMapping()
    {
        CreateMap<DbRefreshLog, DbRefreshLogDto>()
            .ForMember(dto => dto.DbName,
                act => act.MapFrom(m => m.DbRefreshJob != null
                    ? m.DbRefreshJob.DbName : ""))
            .ForMember(dto => dto.GroupCssColor,
                act => act.MapFrom(m =>
                    m.DbRefreshJob != null && m.DbRefreshJob.Group != null
                        ? m.DbRefreshJob.Group.CssColor : ""))
            ;
    }
}
