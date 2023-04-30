﻿using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.GroupChats.Features.InviteToGroupChat;

public record InviteToGroupChatCommand(Guid InviterId, IEnumerable<Guid> InvitedMembers, Guid ChatId) : ICommand<bool>;