using Messenger.Core.Requests.Abstractions;
using Messenger.SubscriptionPlans.Enums;
using Messenger.SubscriptionPlans.Models;

namespace Messenger.Subscriptions.Features.GetSubscriptionPlans;

public record GetSubscriptionPlansQuery : IQuery<Dictionary<Plan, PlanDetails>>;