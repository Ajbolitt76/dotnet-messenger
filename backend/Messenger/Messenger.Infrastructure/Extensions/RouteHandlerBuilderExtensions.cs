using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Messenger.Infrastructure.Extensions;

public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder AddDefaultDescriptions(
        this RouteHandlerBuilder builder,
        bool hasValidation = true,
        bool hasAuthorization = true
    )
    {
        if (hasValidation)
            builder.ProducesProblem(400);

        if (hasAuthorization)
        {
            builder.ProducesProblem(401);
            builder.ProducesProblem(403);
        }

        return builder;
    }
}
