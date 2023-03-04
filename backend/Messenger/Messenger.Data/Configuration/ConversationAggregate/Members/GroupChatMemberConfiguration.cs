using Messenger.Core.Model.ConversationAggregate.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Data.Configuration.ConversationAggregate.Members;

public class GroupChatMemberConfiguration : BaseMemberConfiguration<GroupChatMember>
{
    public override void ConfigureChild(EntityTypeBuilder<GroupChatMember> typeBuilder)
    {
        typeBuilder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        typeBuilder.HasOne(x => x.Conversation)
            .WithMany()
            .HasForeignKey(x => x.ConversationId);
        
        typeBuilder.Property(x => x.WasExcluded)
            .HasDefaultValue(false);

        typeBuilder.Property(x => x.WasBanned)
            .HasDefaultValue(false);

        typeBuilder.HasIndex(x => new {x.ConversationId, x.UserId})
            .IsDescending(false, false)
            .IsUnique();
    }
}
