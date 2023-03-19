using System.Diagnostics.Contracts;
using System.Net;

namespace Soenneker.Extensions.HttpContext;

public static class HttpContextExtension
{
    [Pure]
    public static bool IsLocalRequest(this Microsoft.AspNetCore.Http.HttpContext context)
    {
        if (context.Connection.RemoteIpAddress == null && context.Connection.LocalIpAddress == null)
            return true;

        if (context.Connection.RemoteIpAddress == null) 
            return false;

        if (context.Connection.RemoteIpAddress.Equals(context.Connection.LocalIpAddress))
            return true;

        if (IPAddress.IsLoopback(context.Connection.RemoteIpAddress))
            return true;

        return false;
    }
}