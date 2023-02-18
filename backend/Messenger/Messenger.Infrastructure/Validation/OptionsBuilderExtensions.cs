using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Messenger.Infrastructure.Validation;

public static class OptionsBuilderExtensions
{
    public static OptionsBuilder<TOptions> ValidateWithFluentValidation
        <TOptions>(this OptionsBuilder<TOptions> optionsBuilder) 
        where TOptions : class
    {
        optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>, FluentValidatorValidateOptions<TOptions>>();
        return optionsBuilder;
    }
}
