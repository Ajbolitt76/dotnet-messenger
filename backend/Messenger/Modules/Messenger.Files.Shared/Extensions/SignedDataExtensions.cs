using System.Web;
using Messenger.Crypto.Models;

namespace Messenger.Files.Shared.Extensions;

public static class SignedDataExtensions
{
    public static string ToQuery<T>(this SignedData<T> signedData) where T : FileRequests.FileRequest
    {
        var ub = new UriBuilder(signedData.Data.ToQuery());
        ub.Query += $"&{nameof(SignedData<T>.Signature)}={HttpUtility.UrlEncode(signedData.Signature)}";
        return $"{ub.Path.TrimStart('/').TrimEnd('/')}{ub.Query}";
    }
}
