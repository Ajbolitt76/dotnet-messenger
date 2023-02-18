using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Messenger.Core.Exceptions;
using Messenger.Core.Model.UserAggregate;
using Messenger.Core.Requests.Abstractions;

namespace Messenger.User.Feature.GetProfileMainData;

public class GetProfileMainDataQueryHandler : IQueryHandler<GetProfileMainDataQuery, GetProfileMainDataQueryResponse>
{
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetProfileMainDataQueryHandler(IDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<GetProfileMainDataQueryResponse> Handle(
        GetProfileMainDataQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.RepetUsers.Where(x => x.Id == request.UserId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken)
               ?? throw new NotFoundException<RepetUser>();
        
        return _mapper.Map<GetProfileMainDataQueryResponse>(user);
    }
}
