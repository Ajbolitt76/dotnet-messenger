using Messenger.Core.Model.UserAggregate;

namespace Messenger.User.Feature.GetProfileMainData;

public class GetProfileMainDataQueryResponse
{
    public Guid Id { get; set; }
    public string ProfilePhoto { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public DateTime? DateOfBirth { get; set; }
}