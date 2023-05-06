using Messenger.SubscriptionPlans.Enums;
using Messenger.SubscriptionPlans.Models;

namespace Messenger.SubscriptionPlans.Services;

public static class Plans
{
    public static Dictionary<Plan, PlanDetails> PlansDict =
        new()
        {
            {Plan.Broke, new PlanDetails(2, 200, 500_000, false)},
            {Plan.Standard, new PlanDetails(5, 500, 2_000_000, true)},
            {Plan.Premium, new PlanDetails(10, 10000, 10_000_000, true)},
        };
}
