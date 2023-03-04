namespace Messenger.Core.Model.ConversationAggregate.Permissions;

public enum GroupMemberPermissions
{
    None = 0,
    SendMessages = 1,
    SendFiles = 2,
    ChangeGroupInfo = 4,
    InviteMembers = 8,
    DeleteMessages = 16,
    BanMembers = 32,
    AddAdmins = 64
}
