using MediatR;

namespace Messenger.Core.Requests.Abstractions;

public interface ICommandHandler<in TCommand, TOut> : IRequestHandler<TCommand, TOut>
    where TCommand : ICommand<TOut>
{
}