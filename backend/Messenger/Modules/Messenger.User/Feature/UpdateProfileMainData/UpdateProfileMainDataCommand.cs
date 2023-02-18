using Messenger.Core.Model.UserAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Crypto.Models;
using Messenger.Files.Shared.FileRequests;

namespace Messenger.User.Feature.UpdateProfileMainData;

public record UpdateProfileMainDataCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    DateTime? DateOfBirth,
    Gender Gender,
    Guid? ProfilePicture) : ICommand<bool>;