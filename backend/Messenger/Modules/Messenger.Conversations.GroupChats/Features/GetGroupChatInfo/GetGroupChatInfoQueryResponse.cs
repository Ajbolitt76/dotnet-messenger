namespace Messenger.Conversations.GroupChats.Features.GetGroupChatInfo;

/// <summary>
/// Прикол
/// </summary>
/// <param name="Id">21321</param>
/// <param name="Title">3213</param>
/// <param name="Description">3213</param>
/// <param name="PictureId">3213</param>
/// <param name="LastUpdated">3213</param>
public record GetGroupChatInfoQueryResponse(
    Guid Id,
    string Title,
    string? Description,
    Guid PictureId,
    DateTime LastUpdated);
