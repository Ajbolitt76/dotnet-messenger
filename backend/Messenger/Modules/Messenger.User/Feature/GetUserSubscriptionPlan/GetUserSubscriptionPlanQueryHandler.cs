using MapsterMapper;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.SubscriptionPlans.Enums;
using Messenger.SubscriptionPlans.Services;
using Microsoft.EntityFrameworkCore;

namespace Messenger.User.Feature.GetUserSubscriptionPlan;

public class GetUserSubscriptionPlanQueryHandler : IQueryHandler<GetUserSubscriptionPlanQuery, GetUserSubscriptionPlanQueryResponse>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetUserSubscriptionPlanQueryHandler(IDbContext dbContext, IMapper mapper, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<GetUserSubscriptionPlanQueryResponse> Handle(
        GetUserSubscriptionPlanQuery request,
        CancellationToken cancellationToken)
    {
        var userSubscription = await _dbContext.UsersSubscriptions
            .FirstOrDefaultAsync(us => us.UserId == request.UserId && us.ExpiresAt > _dateTimeProvider.NowUtc);

        if (userSubscription == null)
            return new GetUserSubscriptionPlanQueryResponse();

        var subscriptionDetails = Plans.PlansDict[(Plan)userSubscription.Plan];
        var response = _mapper.Map<GetUserSubscriptionPlanQueryResponse>(subscriptionDetails);
        response.Plan = userSubscription.Plan;
        response.CreatedAt = userSubscription.CreatedAt;
        response.ExpiresAt = userSubscription.ExpiresAt;

        return response;
    }
}
