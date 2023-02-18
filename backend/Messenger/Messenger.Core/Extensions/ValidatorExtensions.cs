using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using Messenger.Core.Exceptions;

namespace Messenger.Core.Extensions;

public static class ValidatorExtensions
{
    public static Dictionary<string, List<PropertyValidationInfo>> ToValidationResult(
        this IEnumerable<ValidationFailure> validationFailures)
    {
        return validationFailures
            .GroupBy(
                x => x.PropertyName,
                x => x,
                (propertyName, failure) => new
                {
                    Key = propertyName,
                    Values = failure.Select(PropertyValidationInfo.FromFailure).ToList()
                })
            .ToDictionary(x => x.Key, x => x.Values);
    }

    public static IRuleBuilderOptions<T, TProperty> WithLocalizationState<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule,
        Func<T, TProperty, Dictionary<string, object>>? additionalState = null)
    {
        Func<ValidationContext<T>, TProperty, Dictionary<string, object>> handler = static (ctx, _) =>
        {
            var dic = ctx.MessageFormatter.PlaceholderValues
                .Where(
                    static x =>
                        x.Key is not (
                            nameof(MessageFormatter.PropertyName)
                            or nameof(MessageFormatter.PropertyValue)))
                .ToDictionary(
                    entry => entry.Key,
                    entry => entry.Value);

            return dic;
        };

        if (additionalState != null)
        {
            var baseHandler = handler;
            handler = (ctx, tp) =>
                baseHandler(ctx, tp)
                    .Concat(additionalState(ctx.InstanceToValidate, tp))
                    .ToDictionary(
                        entry => entry.Key,
                        entry => entry.Value);
        }

        DefaultValidatorOptions
            .Configurable(rule)
            .Current.CustomStateProvider = handler;

        return rule;
    }
}
