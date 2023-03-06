namespace Messenger.Core.Model.ConversationAggregate.Permissions;

[Flags]
public enum ChannelMemberPermissions
{
    None = 0,
    AddPosts = 1,
    DeletePosts = 2,
    AddAdmins = 4
}
