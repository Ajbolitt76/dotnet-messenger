using Mapster;
using Messenger.Files.Shared.FileRequests;
using Messenger.Files.Shared.Services;

namespace Messenger.Files.Shared.Extensions;

public static class FileUtilities
{
    public static string? FileIdToLink<TFile>(Func<Guid, TFile?> mapGuidToStringFile, Guid? guid)
        where TFile : FileRequest
    {
        if (guid == null)
            return null;

        var mappedFile = mapGuidToStringFile(guid.Value);

        if (mappedFile == null)
            return null;

        return MapContext.Current.GetService<IFileUrlService>()
            .GetSignedFileRequestQuery(mappedFile);
    }

    public static ImageFileRequest? MapGuidToImage(
        Guid guid,
        int? Height = null,
        int? Width = null,
        DateTime? Expiry = null)
        => new()
        {
            FileId = guid,
            Height = Height,
            Width = Width,
            Expiry = Expiry
        };

    public static FileRequest? MapGuidToFile(
        Guid guid,
        DateTime? Expiry = null)
        => new()
        {
            FileId = guid,
            Expiry = Expiry
        };
}
