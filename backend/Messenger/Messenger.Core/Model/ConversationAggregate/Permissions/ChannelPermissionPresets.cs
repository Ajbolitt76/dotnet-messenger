namespace Messenger.Core.Model.ConversationAggregate.Permissions;

public abstract class ChannelPermissionPresets
{
    public const ChannelMemberPermissions Creator =
        (ChannelMemberPermissions) int.MaxValue;

    public const ChannelMemberPermissions Admin =
        ChannelMemberPermissions.AddPosts
        | ChannelMemberPermissions.DeletePosts
        | ChannelMemberPermissions.ChangeChannelInfo;

    public const ChannelMemberPermissions Member = ChannelMemberPermissions.None;
}
