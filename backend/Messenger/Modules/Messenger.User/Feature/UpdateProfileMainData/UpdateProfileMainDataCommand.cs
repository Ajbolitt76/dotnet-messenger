using Messenger.Core.Model.UserAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Crypto.Models;
using Messenger.Files.Shared.FileRequests;

namespace Messenger.User.Feature.UpdateProfileMainData;

public record UpdateProfileMainDataCommand(
    Guid UserId,
    string Name,
    DateTime? DateOfBirth,
    Guid? ProfilePicture) : ICommand<bool>;