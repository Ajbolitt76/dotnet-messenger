using Messenger.Conversations.Common.Models;
using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.Common.Abstractions;

public interface IMessageActionHandler<TAction, TResult> : IDomainHandler<TAction, TResult>
    where TAction : class, IMessageAction<TResult>
{
    public static abstract string MessageType { get; }
}
