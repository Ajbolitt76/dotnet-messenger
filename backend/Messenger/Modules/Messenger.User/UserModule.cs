using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Messenger.Core.Model.UserAggregate;
using Messenger.Files.Shared.Extensions;
using Messenger.Infrastructure.Modules;
using Messenger.User.Feature.GetProfileMainData;

namespace Messenger.User;

public class UserModule : IModule
{
    public static void RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        return;
    }

    public class Mappings : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RepetUser, GetProfileMainDataQueryResponse>()
                .Map(
                    x => x.ProfilePhoto,
                    x => FileUtilities.FileIdToLink(
                        y => FileUtilities.MapGuidToImage(y, 380, 240, null), x.ProfilePhotoId));
        }
    }
}
