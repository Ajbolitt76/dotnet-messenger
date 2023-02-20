namespace Messenger.Core.Model.UserAggregate;

public class MessengerUser : BaseEntity
{
    public Guid? ProfilePhotoId { get; set; }
    
    public required string UserName { get; set; }
    
    public required string Name { get; set; }
    
    public required string PhoneNumber { get; set; }

    public DateTime? DateOfBirth { get; set; }
    
    public required Guid IdentityUserId { get; set; }
    
    public MessengerUser()
    {
    }
}