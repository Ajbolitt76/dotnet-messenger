using Messenger.Core.Requests.Abstractions;

namespace Messenger.User.Feature.GetProfileMainData;

public record GetProfileMainDataQuery(Guid UserId) : IQuery<GetProfileMainDataQueryResponse>;