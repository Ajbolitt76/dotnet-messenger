using Messenger.Core.Model.ConversationAggregate.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Data.Configuration.ConversationAggregate.Members;

public class ChannelMemberConfiguration: BaseMemberConfiguration<ChannelMember>
{
    public override void ConfigureChild(EntityTypeBuilder<ChannelMember> typeBuilder)
    {
        typeBuilder.HasIndex(x => new {x.ConversationId, x.UserId})
            .IsDescending(false, false)
            .IsUnique();

        typeBuilder.Property(x => x.IsAdmin)
            .HasDefaultValue(false);
    }
}
