using System.Net;
using FluentValidation.Results;

namespace Messenger.Core.Exceptions;

public record PropertyValidationInfo(string Message, Dictionary<string, object>? LocalizationValues = null)
{
    public static PropertyValidationInfo FromFailure(ValidationFailure failure)
    {
        return new PropertyValidationInfo(failure.ErrorMessage, failure.CustomState as Dictionary<string, object>);
    }
};

public class ValidationFailedException : DomainException
{
    public Dictionary<string, List<PropertyValidationInfo>> Errors { get; } = new();

    public ValidationFailedException(Dictionary<string, List<PropertyValidationInfo>> errors)
        : base(ErrorCodes.ValidationFailedError, (int)HttpStatusCode.BadRequest)
    {
        Errors = errors;
    }

    public ValidationFailedException(string propertyName, string message = "")
        : base(ErrorCodes.ValidationFailedError, (int)HttpStatusCode.BadRequest)
    {
        Errors.Add(propertyName, new List<PropertyValidationInfo> { new(message) });
    }
    
}