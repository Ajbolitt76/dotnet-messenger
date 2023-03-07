using Messenger.Core.Model.ConversationAggregate.Members;

namespace Messenger.Data.Configuration.ConversationAggregate.Members;

public abstract class BaseMemberConfiguration<TEntity> : BaseConfiguration<TEntity>
    where TEntity : BaseMember
{
    
}
