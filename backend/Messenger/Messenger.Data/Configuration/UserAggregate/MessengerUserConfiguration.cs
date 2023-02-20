using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Messenger.Core.Model.UserAggregate;

namespace Messenger.Data.Configuration.UserAggregate;

public class MessngerUserConfiguration : BaseConfiguration<MessengerUser>
{
    public override void ConfigureChild(EntityTypeBuilder<MessengerUser> typeBuilder)
    {
        typeBuilder.HasIndex(x => x.IdentityUserId);
        typeBuilder.HasIndex(x => x.PhoneNumber);
    }
}