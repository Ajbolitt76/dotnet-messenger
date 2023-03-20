namespace Messenger.Core.Model.ConversationAggregate.Permissions;

public class GroupChatPermissionPresets
{
    public static readonly GroupMemberPermissions Creator = 
        (GroupMemberPermissions)int.MaxValue;

    public const GroupMemberPermissions NewMember = 
        GroupMemberPermissions.SendMessages 
        | GroupMemberPermissions.SendFiles 
        | GroupMemberPermissions.ChangeGroupInfo 
        | GroupMemberPermissions.InviteMembers;
}
