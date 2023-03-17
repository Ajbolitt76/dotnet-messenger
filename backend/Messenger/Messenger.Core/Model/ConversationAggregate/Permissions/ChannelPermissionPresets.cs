namespace Messenger.Core.Model.ConversationAggregate.Permissions;

public abstract class ChannelPermissionPresets
{
    public const ChannelMemberPermissions Creator = 
        ChannelMemberPermissions.AddAdmins 
        | ChannelMemberPermissions.AddPosts 
        | ChannelMemberPermissions.DeletePosts;

    public const ChannelMemberPermissions Admin = 
            ChannelMemberPermissions.AddPosts |
            ChannelMemberPermissions.DeletePosts;

    public const ChannelMemberPermissions Member = ChannelMemberPermissions.None;
}
