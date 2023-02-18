using MediatR;

namespace Messenger.Core.Requests.Abstractions;

public interface ICommand<out T> : IRequest<T>
{
    
}

public interface ICommand : ICommand<Unit>
{
    
}