namespace Messenger.Conversations.Channel.Features.GetChannelInfo;

public record GetChannelInfoQueryResponse(
    Guid Id,
    string Title,
    string? Description,
    Guid PictureId,
    DateTime LastUpdated);
