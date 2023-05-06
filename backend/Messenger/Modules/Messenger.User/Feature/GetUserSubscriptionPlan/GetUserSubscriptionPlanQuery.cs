using Messenger.Core.Requests.Abstractions;

namespace Messenger.User.Feature.GetUserSubscriptionPlan;

public record GetUserSubscriptionPlanQuery(Guid UserId) : IQuery<GetUserSubscriptionPlanQueryResponse>;