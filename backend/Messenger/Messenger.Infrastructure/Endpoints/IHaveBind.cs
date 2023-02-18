using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace Messenger.Infrastructure.Endpoints;

public interface IHaveBind<TSelf> where TSelf : IHaveBind<TSelf>
{
    public static abstract ValueTask<TSelf?> BindAsync(HttpContext context, ParameterInfo parameter);
}
