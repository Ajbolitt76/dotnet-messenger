using MediatR;
using Microsoft.EntityFrameworkCore;
using Messenger.Core.Requests.Abstractions;
using Messenger.Data;

namespace Messenger.Api.Behaviours;

public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IRunInTransaction
{
    private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
    private readonly RepetContext _dbContext;
    
    public TransactionBehaviour(RepetContext dbContext, ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var execStrategy = _dbContext.Database.CreateExecutionStrategy();
        return execStrategy.ExecuteAsync(
            async () =>
            {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        
                try
                {
                    _logger.LogInformation($"Begin transaction {typeof(TRequest).Name}");

                    var response = await next();

                    await transaction.CommitAsync(cancellationToken);

                    _logger.LogInformation($"Committed transaction {typeof(TRequest).Name}");

                    return response;
                }
                catch (Exception e)
                {
                    _logger.LogInformation($"Rollback transaction executed {typeof(TRequest).Name}");

                    await transaction.RollbackAsync(cancellationToken);

                    _logger.LogError(e.Message, e.StackTrace);

                    throw;
                }
            });
    }
}
