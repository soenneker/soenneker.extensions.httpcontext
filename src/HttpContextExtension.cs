using System.Diagnostics.Contracts;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Soenneker.Extensions.HttpContext;

/// <summary>
/// A collection of helpful HttpContext extension methods
/// </summary>
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

    public static void SetUnauthorized(this Microsoft.AspNetCore.Http.HttpContext context)
    {
        // TODO: Should probably be safer about these
        // Return authentication type (causes browser to show login dialog)
        context.Response.Headers[HeaderNames.WWWAuthenticate] = "Basic";
        context.Response.Headers[HeaderNames.Authorization] = "";

        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    }
}