using Microsoft.EntityFrameworkCore;
using Messenger.Core.Exceptions;
using Messenger.Core.Model.UserAggregate;
using Messenger.Core.Requests.Abstractions;

namespace Messenger.User.Feature.UpdateProfileMainData;

public class UpdateProfileMainDataCommandHandler : ICommandHandler<UpdateProfileMainDataCommand, bool>
{
    private readonly IDbContext _dbContext;

    public UpdateProfileMainDataCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(UpdateProfileMainDataCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.MessengerUsers.FirstOrDefaultAsync(
                x => x.Id == request.UserId,
                cancellationToken: cancellationToken)
            ?? throw new NotFoundException<MessengerUser>();
        
        user.Name = request.Name;
        user.DateOfBirth = request.DateOfBirth;

        if(request.ProfilePicture != null)
            user.ProfilePhotoId = request.ProfilePicture;
        
        await _dbContext.SaveEntitiesAsync(cancellationToken);

        return true;
    }
}
