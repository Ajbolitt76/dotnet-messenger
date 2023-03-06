using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.ConversationInfos;
using Microsoft.EntityFrameworkCore;
using Messenger.Core.Model.FileAggregate;
using Messenger.Core.Model.UserAggregate;

namespace Messenger.Core.Requests.Abstractions;

public interface IDbContext
{
    public DbSet<MessengerUser> MessengerUsers { get; }
    
    public DbSet<SystemFile> Files { get; }
    
    public DbSet<UploadingFile> UploadingFiles { get; }
    
    public DbSet<Conversation> Conversations { get; }
    
    public DbSet<ConversationMessage> ConversationMessages { get; }
    
    public DbSet<ConversationUserStatus> ConversationUserStatuses { get; }

    public DbSet<GroupChatInfo> GroupChatInfos { get; }
    
    public DbSet<PersonalChatInfo> PersonalChatInfos { get; }

    public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}
