using Demo.DbRefreshManager.WebApi.Models.Frontend;
using Demo.DbRefreshManager.WebApi.Models.Options;

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
