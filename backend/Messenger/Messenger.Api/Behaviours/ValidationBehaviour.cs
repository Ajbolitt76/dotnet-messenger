using FluentValidation;
using MediatR;
using Messenger.Core.Exceptions;
using Messenger.Core.Extensions;

namespace Messenger.Api.Behaviours;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) =>
        _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        
        var errorsDictionary = _validators
            .SelectMany(x => x.Validate(context).Errors)
            .ToValidationResult();

        if (errorsDictionary.Any())
            throw new ValidationFailedException(errorsDictionary);

        return await next();
    }
}