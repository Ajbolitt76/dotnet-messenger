using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Messenger.Core.Exceptions;
using Messenger.Core.Model.UserAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Extensions;

namespace Messenger.User.Feature.GetProfileMainData;

public class GetProfileMainDataQueryHandler : IQueryHandler<GetProfileMainDataQuery, GetProfileMainDataQueryResponse>
{
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTimeProvider;

    public GetProfileMainDataQueryHandler(IDbContext dbContext, IMapper mapper, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<GetProfileMainDataQueryResponse> Handle(
        GetProfileMainDataQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.MessengerUsers.Where(x => x.Id == request.UserId)
                       .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                   ?? throw new NotFoundException<MessengerUser>();
        
        var response = _mapper.Map<GetProfileMainDataQueryResponse>(user);
        
        var subscription = await _dbContext.UsersSubscriptions.FirstOrDefaultAsync(x => x.UserId == user.Id && x.ExpiresAt > _dateTimeProvider.NowUtc);

        if (subscription != null)
            response.SubscriptionPlan = subscription.Plan;
        
        return response;
    }
}
