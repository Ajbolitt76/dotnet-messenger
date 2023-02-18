using Microsoft.AspNetCore.Builder;

namespace Messenger.Infrastructure.Validation.ValidationFilter;

public readonly struct ValidationBuilder<TBuilder> where TBuilder : IEndpointConventionBuilder
{
    private readonly TBuilder Builder;
    private readonly List<Type> _registrations;
    
    public ValidationBuilder(TBuilder builder)
    {
        Builder = builder;
        _registrations = new List<Type>();
    }

    public ValidationBuilder<TBuilder> AddFor<TFor>()
    {
        _registrations.Add(typeof(TFor));
        return this;
    }

    public TBuilder SetValidation()
    {
        Builder.AddValidationFilter(_registrations.ToArray());
        return Builder;
    }
}
