using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Messenger.Core.Model.UserAggregate;

namespace Messenger.Data.Configuration.UserAggregate;

public class MessngerUserConfiguration : BaseConfiguration<MessengerUser>
{
    public override void ConfigureChild(EntityTypeBuilder<MessengerUser> typeBuilder)
    {
        typeBuilder
            .HasMany(x => x.PersonalChatMembership)
            .WithOne(x => x.MessengerUser)
            .HasForeignKey(x => x.UserId);

        typeBuilder
            .HasMany(x => x.GroupChatMemberships)
            .WithOne(x => x.MessengerUser)
            .HasForeignKey(x => x.UserId);
        
        typeBuilder
            .HasMany(x => x.ChannelMemberships)
            .WithOne(x => x.MessengerUser)
            .HasForeignKey(x => x.UserId);

        typeBuilder.HasIndex(x => x.IdentityUserId);
        typeBuilder.HasIndex(x => x.PhoneNumber);
    }
}