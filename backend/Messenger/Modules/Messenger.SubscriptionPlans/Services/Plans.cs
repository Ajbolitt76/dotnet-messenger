using Messenger.SubscriptionPlans.Enums;
using Messenger.SubscriptionPlans.Models;

namespace Messenger.SubscriptionPlans.Services;

public static class Plans
{
    public static Dictionary<Plan, PlanDetails> PlansDict =
        new()
        {
            {Plan.Broke, new PlanDetails(0, 100, 1000, false)},
            {Plan.Light, new PlanDetails(2, 200, 10_000, false)},
            {Plan.Standard, new PlanDetails(5, 500, 100_000, true)},
            {Plan.Premium, new PlanDetails(10, 10000, 1_000_000, true)},
        };
}
