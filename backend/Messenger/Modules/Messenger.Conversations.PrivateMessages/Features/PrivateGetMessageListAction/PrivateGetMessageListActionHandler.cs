﻿using Messenger.Conversations.Common.Abstractions;
using Messenger.Conversations.Common.Features.ReserveConversationNumberCommand;
using Messenger.Conversations.Common.MessageActions.GetMessageList;
using Messenger.Conversations.PrivateMessages.Models;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Conversations.PrivateMessages.Features.PrivateGetMessageListAction;

public class PrivateGetMessageListActionHandler
    : IMessageActionHandler<GetMessageListAction, GetMessageListActionResponse>
{
    private readonly IDbContext _dbContext;
    private readonly IUserService _userService;
    public static string MessageType => ConversationTypes.PersonalChat;

    public PrivateGetMessageListActionHandler(
        IDbContext dbContext,
        IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    public async Task<GetMessageListActionResponse> Handle(
        GetMessageListAction request,
        CancellationToken cancellationToken)
    {
        var messages = await _dbContext.ConversationMessages
            .Where(x => x.ConversationId == request.ConversationId)
            .OrderBy(x => x.Position)
            .Take(request.Count)
            .Select(
                x => new PrivateMessageListProjection(
                ))
            .ToListAsync(cancellationToken: cancellationToken);

        return new(messages);
    }
}
