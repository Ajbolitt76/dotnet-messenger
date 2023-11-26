using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Messenger.Core.Model.FileAggregate;
using Messenger.Core.Model.FileAggregate.FileLocation;
using Messenger.Core.Requests.Abstractions;
using Messenger.Crypto.Models;
using Messenger.Crypto.Services;
using Messenger.Files.Shared;
using Messenger.Files.Shared.FileRequests;
using tusdotnet.Stores;

namespace Messenger.Files.Services;

public class FileServer
{
    private static readonly Dictionary<Type, Type> CachedLocationServers = new();

    private readonly IDbContext _dbContext;
    private readonly IServiceProvider _provider;
    private readonly ModuleSignatureService<FileCoreModule> _signatureService;

    public FileServer(
        IDbContext dbContext,
        IServiceProvider provider,
        ModuleSignatureService<FileCoreModule> signatureService)
    {
        _dbContext = dbContext;
        _provider = provider;
        _signatureService = signatureService;
    }

    public async Task<IResult> ServeFileAsync<T>(
        SignedData<T> fileRequestWithSignature,
        HttpContext context,
        CancellationToken cancellationToken = default) where T : FileRequest
    {
        var fileRequest = fileRequestWithSignature.Data;

        if (!_signatureService.Validate(fileRequestWithSignature) || fileRequestWithSignature.Data.Expiry < DateTime.UtcNow)
            return Results.Forbid();

        var systemFile = await _dbContext.Files
            .FirstOrDefaultAsync(x => x.Id == fileRequest.FileId, cancellationToken);
        
        if (systemFile is null)
            return Results.NotFound();

        var server =
            _provider.GetService(GetCachedLocationServer(systemFile.FileLocation.GetType())) as IFileLocationServer
            ?? throw new InvalidOperationException("File location server not found");

        return await server.ServeFileAsync(systemFile, fileRequest, context, cancellationToken);
    }
    
    private Type GetCachedLocationServer(Type locationType)
    {
        if (CachedLocationServers.ContainsKey(locationType))
            return CachedLocationServers[locationType];

        var locationServerType = typeof(IFileLocationServer<>).MakeGenericType(locationType);
        CachedLocationServers.Add(locationType, locationServerType);
        return locationServerType;
    }
}
