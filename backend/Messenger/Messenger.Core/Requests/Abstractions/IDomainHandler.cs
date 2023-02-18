using MediatR;

namespace Messenger.Core.Requests.Abstractions;

public interface IDomainHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
}
