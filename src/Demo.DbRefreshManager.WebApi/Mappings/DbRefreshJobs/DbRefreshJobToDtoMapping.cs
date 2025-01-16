using AutoMapper;
using Demo.DbRefreshManager.Common.Extensions;
using Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.WebApi.Models.DbRefreshJobs;

namespace Demo.DbRefreshManager.WebApi.Mappings.DbRefreshJobs;

/// <summary>
/// Профиль конвертации доменной модели задачи перезаливки БД в dto.
/// </summary>
public class DbRefreshJobToDtoMapping : Profile
{
    /// <inheritdoc cref="DbRefreshJobToDtoMapping" />
    public DbRefreshJobToDtoMapping()
    {
        CreateMap<DbRefreshJob, DbRefreshJobDto>()
            .ForMember(dto => dto.ScheduleRefreshDate,
                act => act.MapFrom(m =>
                    m.ScheduleRefreshTime.UtcTimeToToday() > DateTime.UtcNow
                        ? m.ScheduleRefreshTime.UtcTimeToToday()
                        : m.ScheduleRefreshTime.UtcTimeToToday().AddDays(1)))
            .ForMember(dto => dto.ScheduleChangeUser,
                act => act.MapFrom(m => m.ScheduleChangeUser == null
                    ? null : m.ScheduleChangeUser.LdapLogin))
            .ForMember(dto => dto.GroupSortOrder,
                act => act.MapFrom(m => m.Group == null ? default : m.Group.SortOrder))
            .ForMember(dto => dto.GroupCssColor,
                act => act.MapFrom(m => m.Group == null ? "" : m.Group.CssColor))
            .ForMember(dto => dto.GroupAccessRoles,
                act => act.MapFrom(m => m.Group == null ? new(0)
                    : m.Group.AccessRoles.Select(r => r.Name).ToList()))
            ;
    }
}
