using Microsoft.AspNetCore.Mvc;
using Messenger.Core.Exceptions;

namespace Messenger.Infrastructure.Exceptions;

public static class DomainExceptionToProblemDetails
{
    public static ProblemDetails ToProblemDetails(this DomainException ex)
    {
        var problems = new ProblemDetails
        {
            Title = ex.Message,
            Status = ex.StatusCode,
            Extensions =
            {
                ["placeholderData"] = ex.PlaceholderData,
            }
        };
            
        if(ex is ValidationFailedException val)
            problems.Extensions["errors"] = val.Errors;
        
        return problems;
    }
}