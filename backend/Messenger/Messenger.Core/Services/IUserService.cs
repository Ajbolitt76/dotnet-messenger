using Messenger.Core.Model;
using Messenger.Core.Model.UserAggregate;

namespace Messenger.Core.Services;

public interface IUserService
{
    public Guid? UserId { get; }

    public bool IsAuthenticated { get; }
}