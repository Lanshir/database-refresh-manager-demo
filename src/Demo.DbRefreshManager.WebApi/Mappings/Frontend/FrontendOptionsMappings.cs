using Demo.DbRefreshManager.Common.Config.Concrete;
using Demo.DbRefreshManager.WebApi.Models.Frontend;

namespace Demo.DbRefreshManager.WebApi.Mappings.Frontend;

public static class FrontendOptionsMappings
{
    extension(FrontendConfig src)
    {
        /// <summary>
        /// Конвертация параметров frontend в dto.
        /// </summary>
        public FrontendConfigDto ToDto() => new(
            ObjectsListUrl: src.ObjectsListUrl,
            InstructionUrl: src.InstructionUrl);
    }
}
