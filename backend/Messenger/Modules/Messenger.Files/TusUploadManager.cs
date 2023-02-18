using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Messenger.Core.Model.FileAggregate;
using Messenger.Core.Model.FileAggregate.FileLocation;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Crypto.Services;
using Messenger.Files.Shared.FileRequests;
using tusdotnet.Interfaces;
using tusdotnet.Models;
using tusdotnet.Models.Configuration;
using tusdotnet.Models.Expiration;
using tusdotnet.Stores;

namespace Messenger.Files;
public partial class TusUploadManager
{
    private readonly IDbContext _dbContext;
    private readonly IUserService _userService;
    private readonly ICryptoService _cryptoService;
    private readonly ILogger<TusUploadManager> _logger;
    private readonly FileModuleOptions _config;
    private readonly JsonSerializerOptions _jsonConfig;

    public TusUploadManager(
        IDbContext dbContext,
        IOptions<FileModuleOptions> options,
        IOptions<JsonSerializerOptions> jsonOptions,
        IUserService userService,
        ICryptoService cryptoService,
        ILogger<TusUploadManager> logger)
    {
        _dbContext = dbContext;
        _userService = userService;
        _cryptoService = cryptoService;
        _jsonConfig = jsonOptions.Value;
        _logger = logger;
        _config = options.Value;
    }

    // TC?
    public TusDiskStore GetStore()
    {
        var uploadFolder = _config.FileStoragePath;
        Directory.CreateDirectory(uploadFolder);
        return new(uploadFolder, true);
    }

    public DefaultTusConfiguration BuildConfiguration() => new DefaultTusConfiguration
    {
        Expiration = new SlidingExpiration(TimeSpan.FromSeconds(_config.Expiration)),
        Store = GetStore(),
        AllowedExtensions = new TusExtensions(
            TusExtensions.Creation,
            TusExtensions.Checksum,
            TusExtensions.Concatenation,
            TusExtensions.ChecksumTrailer,
            TusExtensions.Expiration),
        UsePipelinesIfAvailable = true,
        MaxAllowedUploadSizeInBytesLong = _config.MaxAllowedUploadSizeInBytesLong,
        Events = new Events
        {
            OnAuthorizeAsync = AuthorizeAsync,
            OnBeforeCreateAsync = BeforeCreateAsync,
            OnCreateCompleteAsync = CreateCompleteAsync,
            OnFileCompleteAsync = FileCompleteAsync,
        }
    };

    private async Task AuthorizeAsync(AuthorizeContext ctx)
    {
        var file = ctx.FileId;
    }

    private async Task BeforeCreateAsync(BeforeCreateContext ctx)
    {
        var fileName = ctx.Metadata.GetValueOrDefault(_config.FileNameMetadataKey)?.GetString(Encoding.UTF8);

        if (fileName == null)
            ctx.FailRequest("File name is required");
    }

    private async Task CreateCompleteAsync(CreateCompleteContext ctx)
    {
        var fileName = ctx.Metadata.GetValueOrDefault(_config.FileNameMetadataKey)?.GetString(Encoding.UTF8);
        var fileInfo = new UploadingFile(ctx.FileId, fileName, ctx.UploadLength)
        {
            CreatorIp = ctx.HttpContext.Connection.RemoteIpAddress
        };
        _dbContext.UploadingFiles.Add(fileInfo);

        await _dbContext.SaveEntitiesAsync();
        Log.FileCreated(_logger, _userService.UserId, ctx.FileId, ctx.UploadLength);
    }

    private async Task FileCompleteAsync(FileCompleteContext arg)
    {
        var uploadingFile = await _dbContext.UploadingFiles.FirstOrDefaultAsync(x => x.TusId == arg.FileId);

        if (uploadingFile == null)
        {
            Log.UnknownFileCompleted(_logger, _userService.UserId, arg.FileId);
            return;
        }

        var file = new SystemFile(uploadingFile, new TusFileLocation(arg.FileId));

        _dbContext.Files.Add(file);
        _dbContext.UploadingFiles.Remove(uploadingFile);

        await _dbContext.SaveEntitiesAsync();

        var result = _cryptoService.SignObject(
            new FileOwnership(file.Id, "unspecified"),
            _config.LinkSigningKeyPair.PrivateKeyBytes!);
        
        var encodedResult = Convert.ToBase64String(
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(result, _jsonConfig)));

        arg.HttpContext.Response.Headers.Add("SystemFile-Signed", encodedResult);

        Log.FileCompleted(_logger, _userService.UserId, arg.FileId);
    }

    internal static partial class Log
    {
        [LoggerMessage(
            1,
            LogLevel.Information,
            "User {UserId} created file {FileId} with size {Size}",
            EventName = "FileCreated")]
        public static partial void FileCreated(ILogger logger, Guid? userId, string fileId, long size);

        [LoggerMessage(
            2,
            LogLevel.Debug,
            "User {UserId} uploaded chunk {ChunkNumber} of file {FileId}",
            EventName = "FileChunkUploaded")]
        public static partial void FileChunkUploaded(ILogger logger, Guid? userId, int chunkNumber, string fileId);

        [LoggerMessage(
            3,
            LogLevel.Information,
            "User {UserId} completed upload of file {FileId}",
            EventName = "FileUploaded")]
        public static partial void FileCompleted(ILogger logger, Guid? userId, string fileId);


        [LoggerMessage(
            3,
            LogLevel.Warning,
            "User {UserId} completed upload of file {FileId}, but it was not found in database",
            EventName = "UnknownFileUploaded")]
        public static partial void UnknownFileCompleted(ILogger logger, Guid? userId, string fileId);
    }
}
