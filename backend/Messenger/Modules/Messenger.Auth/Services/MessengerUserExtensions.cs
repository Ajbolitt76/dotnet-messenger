using Messenger.Core.Model.UserAggregate;
using Messenger.Infrastructure.User;

namespace Messenger.Auth.Services;

public static class MessengerUserExtensions
{
    public static void ReconcileWithIdentity(this MessengerUser messengerUser, ApplicationUser user)
    {
        messengerUser.PhoneNumber = user.PhoneNumber;
        messengerUser.UserName = user.UserName;
        messengerUser.IdentityUserId = user.Id;
    }
}