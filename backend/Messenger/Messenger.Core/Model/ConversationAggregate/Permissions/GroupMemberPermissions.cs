namespace Messenger.Core.Model.ConversationAggregate.Permissions;

[Flags]
public enum GroupMemberPermissions
{
    None = 0,
    SendMessages = 1,
    SendFiles = 2,
    ChangeGroupInfo = 4,
    InviteMembers = 8,
    DeleteMessages = 16,
    MuteMembers = 32,
    BanMembers = 64,
    AddAdmins = 128
}