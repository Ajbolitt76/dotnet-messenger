namespace Messenger.Core.Model.ConversationAggregate.ConversationInfos;

public class ChannelInfo : BaseConversationInfo, IHaveDiscriminator
{
    public static string Discriminator => ConversationTypes.Channel;
}
