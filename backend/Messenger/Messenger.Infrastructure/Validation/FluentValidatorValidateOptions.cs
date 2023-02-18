using FluentValidation;
using FluentValidation.Resources;
using Microsoft.Extensions.Options;

namespace Messenger.Infrastructure.Validation;

public class FluentValidatorValidateOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
{
    private readonly IValidator<TOptions>[] _validators;

    public FluentValidatorValidateOptions(IEnumerable<IValidator<TOptions>> validator)
    {
        _validators = validator.ToArray();
    }
    
    /// <inheritdoc />
    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        var oldLanguageManager = ValidatorOptions.Global.LanguageManager;
        ValidatorOptions.Global.LanguageManager = new LanguageManager();
        
        List<string> failures = new();
        
        foreach (var validator in _validators)
            validator.Validate(options).Errors
                .ForEach(f => failures.Add(f.ErrorMessage));
        
        if(failures.Count > 0)
            return ValidateOptionsResult.Fail(failures);

        ValidatorOptions.Global.LanguageManager = oldLanguageManager;
        
        return ValidateOptionsResult.Success;
    }
}
