using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.Common.Abstractions;

public interface IMessageAction<out TResult> : ICommand<TResult>
{
    
}
