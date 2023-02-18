using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using Messenger.Infrastructure.User;
using Messenger.Data.Extensions;

namespace Messenger.Data.Configuration;

public class UserConfiguration : DependencyInjectedEntityConfiguration<ApplicationUser>
{
    private readonly JsonOptions _jsonOptions;

    public UserConfiguration(IOptions<JsonOptions> jsonOptions)
    {
        _jsonOptions = jsonOptions.Value;
    }
    
    public override void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(x => x.RefreshTokens)
            .HasJsonConversion<IReadOnlyList<RefreshToken>, List<RefreshToken>>(_jsonOptions.SerializerOptions);
    }
}