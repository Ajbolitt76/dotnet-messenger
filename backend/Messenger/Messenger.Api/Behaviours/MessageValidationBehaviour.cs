using MediatR;
using Messenger.Conversations.Features.SendMessageCommand;
using Messenger.Core.Model.ConversationAggregate.Attachment;
using Messenger.Core.Model.SubscriptionAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.SubscriptionPlans.Enums;
using Messenger.SubscriptionPlans.Services;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Api.Behaviours;

public class MessageValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IDbContext _dbContext;

    public MessageValidationBehaviour(IDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not SendMessageCommand sendMessageCommand)
            return await next();

        var userSubscription = await _dbContext.UsersSubscriptions.FirstOrDefaultAsync(
            x => x.UserId == sendMessageCommand.UserId && x.ExpiresAt > _dateTimeProvider.NowUtc, cancellationToken);

        var subscriptionPlan = userSubscription is null ? Plan.Broke : (Plan) userSubscription.Plan;
        
        ValidateMessageLength(subscriptionPlan, sendMessageCommand.TextContent);
        /*ValidateMessageAttachmentsCost(subscriptionPlan, sendMessageCommand.Attachments);*/

        return await next();
    }

    private void ValidateMessageLength(Plan plan, string message)
    {
        var planDetails = Plans.PlansDict[plan];
        if (message.Length > planDetails.MessageCharsLimit)
            throw new UnauthorizedAccessException(
                $"OVER_{Plans.PlansDict[0].MessageCharsLimit}_MESSAGE_LENGTH_FORBIDDEN");
    }

    private void ValidateMessageAttachmentsCost(Plan plan, IEnumerable<IAttachment> attachments)
    {
        var planDetails = Plans.PlansDict[plan];
        if (attachments.Sum(a => a.Cost) > planDetails.AttachmentsCostPerMessage)
            throw new UnauthorizedAccessException(
                $"OVER_{planDetails.AttachmentsCostPerMessage}KB_ATTACHMENTS_FORBIDDEN");
    }
}
