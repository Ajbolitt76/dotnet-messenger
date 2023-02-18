using System.Reflection;
using Microsoft.AspNetCore.Identity;

namespace Messenger.Data.Identity.CustomIdentityErrors;

public class BaseIdentityError : IdentityError
{
    public int StatusCode { get; set; } 
}
