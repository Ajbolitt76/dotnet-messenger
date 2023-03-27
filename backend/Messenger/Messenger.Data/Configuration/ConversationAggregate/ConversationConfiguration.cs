using Messenger.Core.Model.ConversationAggregate;
using Messenger.Data.Extensions;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace Messenger.Data.Configuration.ConversationAggregate;

public class ConversationConfiguration : BaseConfiguration<Conversation>
{
    private readonly IOptions<JsonOptions> _jsonOptions;

    public ConversationConfiguration(IOptions<JsonOptions> jsonOptions)
    {
        _jsonOptions = jsonOptions;
    }
    
    public override void ConfigureChild(EntityTypeBuilder<Conversation> typeBuilder)
    {
        typeBuilder.Ignore(x => x.ConversationInfo);
        
        typeBuilder.Property(x => x.HardDeletedCount)
            .HasDefaultValue(0);
        
    }
}

