using Demo.DbRefreshManager.WebApi.Infrastructure.Options;
using Demo.DbRefreshManager.WebApi.Models.Frontend;

namespace Demo.DbRefreshManager.WebApi.Mappings.Frontend;

public static class FrontendOptionsMappings
{
    extension(FrontendOptions src)
    {
        /// <summary>
        /// Конвертация параметров frontend в dto.
        /// </summary>
        public FrontendConfigDto ToDto() => new(
            ObjectsListUrl: src.ObjectsListUrl,
            InstructionUrl: src.InstructionUrl);
    }
}
