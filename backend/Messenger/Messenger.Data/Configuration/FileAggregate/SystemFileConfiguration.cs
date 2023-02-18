using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using Messenger.Core.Model.FileAggregate;
using Messenger.Data.Extensions;

namespace Messenger.Data.Configuration.FileAggregate;

public class SystemFileConfiguration : BaseFileInfoConfiguration<SystemFile>
{
    private readonly JsonOptions _jsonOptions;

    public SystemFileConfiguration(IOptions<JsonOptions> jsonOptions)
    {
        _jsonOptions = jsonOptions.Value;
    }
    
    public override void ConfigureChild(EntityTypeBuilder<SystemFile> typeBuilder)
    {
        base.ConfigureChild(typeBuilder);
        typeBuilder.Property(x => x.FileLocation)
            .HasJsonConversion(_jsonOptions.SerializerOptions);
    }
}
