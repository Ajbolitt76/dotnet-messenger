using Messenger.Core.Model.UserAggregate;
using Messenger.Infrastructure.User;

namespace Messenger.Auth.Services;

public static class RepetUserExtensions
{
    public static void ReconcileWithIdentity(this RepetUser repetUser, ApplicationUser user)
    {
        repetUser.PhoneNumber = user.PhoneNumber;
        repetUser.Email = user.Email;
        repetUser.UserName = user.UserName;
        repetUser.IdentityUserId = user.Id;
    }
}