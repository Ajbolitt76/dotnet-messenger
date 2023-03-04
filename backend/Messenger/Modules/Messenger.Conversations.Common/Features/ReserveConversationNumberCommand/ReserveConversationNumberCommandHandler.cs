using Messenger.Conversations.Common.Extensions;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Conversations.Common.Features.ReserveConversationNumberCommand;

public class ReserveConversationNumberCommandHandler : IDomainHandler<ReserveConversationNumberCommand, uint>
{
    private readonly IRedisKeyStore _redisKeyStore;
    private readonly IDbContext _dbContext;

    public ReserveConversationNumberCommandHandler(IRedisKeyStore redisKeyStore, IDbContext dbContext)
    {
        _redisKeyStore = redisKeyStore;
        _dbContext = dbContext;
    }

    public async Task<uint> Handle(ReserveConversationNumberCommand request, CancellationToken cancellationToken)
    {
        var key = GetKey(request.ConversationId);

        // Double-check
        if (!await _redisKeyStore.ExistsAsync<uint>(key))
        {
            await using var locked =
                await _redisKeyStore.LockAsync(ConversationQueries.GetConversationLock(request.ConversationId));

            if (!await _redisKeyStore.ExistsAsync<uint>(key))
            {
                var position =
                    await _dbContext.ConversationMessages.GetLatestMessagePositionAsync(request.ConversationId) + 1;
                await _redisKeyStore.SetAsync(key, position);
                return position;
            }
        }
        
        return (uint)await _redisKeyStore.IncrementAsync(key);
    }

    private string GetKey(Guid conversationId) => $"conversation:{conversationId}:messageNumber";
}
