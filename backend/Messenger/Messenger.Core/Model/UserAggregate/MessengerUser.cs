using Messenger.Core.Model.ConversationAggregate.Members;

namespace Messenger.Core.Model.UserAggregate;

public class MessengerUser : BaseEntity
{
    public MessengerUser()
    {
    }

    public Guid? ProfilePhotoId { get; set; }

    public required string UserName { get; set; }

    public required string Name { get; set; }

    public required string PhoneNumber { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public required Guid IdentityUserId { get; set; }

    #region Navigation

    public IReadOnlyList<ChannelMember> ChannelMemberships { get; private set; }

    public IReadOnlyList<GroupChatMember> GroupChatMemberships { get; private set; }

    public IReadOnlyList<PersonalChatMember> PersonalChatMembership { get; private set; }

    #endregion
}
