namespace Messenger.Core.Model.UserAggregate;

public class RepetUser : BaseEntity
{
    public Guid? ProfilePhotoId { get; set; }
    
    public required string UserName { get; set; }
    
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public required string PhoneNumber { get; set; }
    
    public string? Email { get; set; }

    public Gender Gender { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
    
    public required Guid IdentityUserId { get; set; }
    
    public RepetUser()
    {
    }
    
    
}