namespace Messenger.Conversations.GroupChats.Features.GetGroupChatInfo;

public record GetGroupChatInfoQueryResponse(
    Guid Id,
    string Title,
    string? Description,
    Guid PictureId,
    DateTime LastUpdated);
