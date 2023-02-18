using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Messenger.Core.Model.UserAggregate;

namespace Messenger.Data.Configuration.UserAggregate;

public class RepetUserConfiguration : BaseConfiguration<RepetUser>
{
    public override void ConfigureChild(EntityTypeBuilder<RepetUser> typeBuilder)
    {
        typeBuilder.Property(x => x.Gender)
            .HasDefaultValue(Gender.NotStated);
        
        typeBuilder.HasIndex(x => x.IdentityUserId);
        typeBuilder.HasIndex(x => x.PhoneNumber);
    }
}