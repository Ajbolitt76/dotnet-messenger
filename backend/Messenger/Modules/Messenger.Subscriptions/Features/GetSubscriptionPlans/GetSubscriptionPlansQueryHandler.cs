using Messenger.Core.Requests.Abstractions;
using Messenger.SubscriptionPlans.Enums;
using Messenger.SubscriptionPlans.Models;
using Messenger.SubscriptionPlans.Services;

namespace Messenger.Subscriptions.Features.GetSubscriptionPlans;

public class GetSubscriptionPlansQueryHandler : IQueryHandler<GetSubscriptionPlansQuery, Dictionary<Plan, PlanDetails>>
{
    public async Task<Dictionary<Plan, PlanDetails>> Handle(
        GetSubscriptionPlansQuery request,
        CancellationToken cancellationToken) => 
        await Task.FromResult(Plans.PlansDict);
    
}
