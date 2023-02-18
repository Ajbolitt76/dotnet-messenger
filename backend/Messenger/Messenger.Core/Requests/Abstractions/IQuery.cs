using MediatR;

namespace Messenger.Core.Requests.Abstractions;

public interface IQuery<out T> : IRequest<T>
{

}
