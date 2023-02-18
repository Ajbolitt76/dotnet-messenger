using MediatR;

namespace Messenger.Core.Requests.Abstractions;

public interface IQueryHandler<in TQuery, TOut> : IRequestHandler<TQuery, TOut>
    where TQuery : IQuery<TOut>
{
    
}