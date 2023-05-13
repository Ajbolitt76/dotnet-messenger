using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.ConversationInfos;
using Messenger.Core.Model.ConversationAggregate.Members;
using Microsoft.EntityFrameworkCore;
using Messenger.Core.Model.FileAggregate;
using Messenger.Core.Model.UserAggregate;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Messenger.Core.Requests.Abstractions;

public interface IDbContext
{
    DatabaseFacade Database { get; }
    
    DbSet<MessengerUser> MessengerUsers { get; }

    DbSet<SystemFile> Files { get; }

    DbSet<UploadingFile> UploadingFiles { get; }

    DbSet<Conversation> Conversations { get; }

    DbSet<ConversationMessage> ConversationMessages { get; }

    DbSet<ConversationUserStatus> ConversationUserStatuses { get; }

    DbSet<GroupChatInfo> GroupChatInfos { get; }

    DbSet<PersonalChatInfo> PersonalChatInfos { get; }

    DbSet<ChannelInfo> ChannelInfos { get; }

    DbSet<GroupChatMember> GroupChatMembers { get; }

    DbSet<ChannelMember> ChannelMembers { get; }

    DbSet<PersonalChatMember> PersonalChatMembers { get; }

    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}
