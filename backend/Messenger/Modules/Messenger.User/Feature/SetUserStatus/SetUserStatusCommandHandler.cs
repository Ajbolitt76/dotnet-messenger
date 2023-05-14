using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Extensions;
using Messenger.SubscriptionPlans.Enums;
using Messenger.SubscriptionPlans.Services;
using Microsoft.EntityFrameworkCore;

namespace Messenger.User.Feature.SetUserStatus;

public class SetUserStatusCommandHandler : ICommandHandler<SetUserStatusCommand, bool>
{
    private readonly IDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    public SetUserStatusCommandHandler(IDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<bool> Handle(SetUserStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.MessengerUsers.FirstOrNotFoundAsync(u => u.Id == request.UserId, cancellationToken);
        if (!await IsStatusPermitted(request.UserId))
        {
            if (user.Status is not null or "")
                user.Status = null;
            throw new UnauthorizedAccessException("NOT_PERMITTED");
        }
        
        user.Status = request.Status;
        
        return await _dbContext.SaveEntitiesAsync(cancellationToken);
    }

    private async Task<bool> IsStatusPermitted(Guid userId)
    {
        var userSubscription = await _dbContext.UsersSubscriptions.FirstOrDefaultAsync(
            us => us.UserId == userId && us.ExpiresAt > _dateTimeProvider.NowUtc);
        return userSubscription != null && Plans.PlansDict[(Plan)userSubscription.Plan].IsStatusEnabled;
    }

}
