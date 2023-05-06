using Messenger.Core.Model.SubscriptionAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Subscriptions.Features.PurchaseSubscriptionCommand;

public class PurchaseSubscriptionCommandHandler : ICommandHandler<PurchaseSubscriptionCommand, bool>
{
    private readonly IDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public PurchaseSubscriptionCommandHandler(IDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<bool> Handle(PurchaseSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var userSubscription = await _dbContext.UsersSubscriptions
                                   .FirstOrDefaultAsync(us => us.UserId == request.UserId, cancellationToken)
                               ?? new UserSubscription
                               {
                                   UserId = request.UserId
                               };
        userSubscription.CreatedAt = _dateTimeProvider.NowUtc;
        userSubscription.ExpiresAt = _dateTimeProvider.NowUtc.AddMonths((int)request.Term);
        userSubscription.Plan = request.Plan;

        if (!await _dbContext.UsersSubscriptions
                .AnyAsync(us => us.UserId == request.UserId, cancellationToken))
            await _dbContext.UsersSubscriptions.AddAsync(userSubscription, cancellationToken);

        return await _dbContext.SaveEntitiesAsync(cancellationToken);
    }
}
