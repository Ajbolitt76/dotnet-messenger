using Messenger.Core.Model.SubscriptionAggregate;
using Messenger.Core.Model.UserAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Data.Configuration.UserSubscriptionAggregate;

public class UserSubscriptionConfiguration : BaseConfiguration<UserSubscription>
{
    public override void ConfigureChild(EntityTypeBuilder<UserSubscription> typeBuilder)
    {
        typeBuilder.HasOne<MessengerUser>()
            .WithOne()
            .HasForeignKey<UserSubscription>(pi => pi.UserId);
    }
}
