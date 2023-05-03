namespace Messenger.Conversations.GroupChats.Models;

public record NotAddedUser(Guid UserId, string Message);

public class NotAddedUsers
{
    public NotAddedUsers(IEnumerable<Guid> ids, string message)
    {
        Add(ids, message);
    }

    public void Add(IEnumerable<Guid> ids, string message)
    {
        NotAddedUsersList.AddRange(ids.Select(x => new NotAddedUser(x, message)));
    }
    
    public void Add(Guid id, string message)
    {
        NotAddedUsersList.Add(new NotAddedUser(id, message));
    }
    
    public List<NotAddedUser> NotAddedUsersList { get; set; } = new();
}
