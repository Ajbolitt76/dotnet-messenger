using Messenger.Core.Model.UserAggregate;

namespace Messenger.User.Feature.GetProfileMainData;

// public record GetProfileMainDataQueryResponse(
//     string FirstName,
//     string LastName,
//     DateTime? DateOfBirth
// );

public class GetProfileMainDataQueryResponse
{
    public string ProfilePhoto { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender Gender { get; set; }
}