﻿using System.Diagnostics.Contracts;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Soenneker.Extensions.HttpContext;

/// <summary>
/// A collection of helpful HttpContext extension methods
/// </summary>
public static class HttpContextExtension
{
    /// <summary>
    /// Determines whether the request is coming from a local address.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    /// <returns>
    /// <c>true</c> if the request is local; otherwise, <c>false</c>. A request is considered local
    /// if the remote IP address is either not set, the same as the local IP address, or a loopback address.
    /// </returns>
    /// <remarks>
    /// This method checks the connection's remote IP address against the local IP address and loopback addresses
    /// to determine if the request originated from the same machine. It's particularly useful for scenarios
    /// where you want to apply logic only to requests made from the server itself or during development.
    /// </remarks>
    [Pure]
    public static bool IsLocalRequest(this Microsoft.AspNetCore.Http.HttpContext context)
    {
        IPAddress? remoteIp = context.Connection.RemoteIpAddress;
        IPAddress? localIp = context.Connection.LocalIpAddress;

        // If both RemoteIpAddress and LocalIpAddress are null, it's considered a local request
        // Otherwise, if RemoteIpAddress is null but LocalIpAddress is not, it's not a local request
        if (remoteIp == null) 
            return localIp == null;

        // Check if the remote IP is the same as the local IP or is a loopback address
        return remoteIp.Equals(localIp) || IPAddress.IsLoopback(remoteIp);
    }

    /// <summary>
    /// Sets the response to indicate that the request is unauthorized.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    /// <remarks>
    /// This method updates the response to indicate that the request requires authentication.
    /// It sets the WWW-Authenticate header to "Basic", prompting clients to provide credentials.
    /// The method ensures that it does not overwrite any existing WWW-Authenticate header.
    /// Use this method to challenge clients for authentication when accessing protected resources.
    /// Note: The Authorization header is typically used in requests rather than responses; thus,
    /// this method does not set the Authorization header in the response.
    /// </remarks>
    public static void SetUnauthorized(this Microsoft.AspNetCore.Http.HttpContext context)
    {
        if (!context.Response.Headers.ContainsKey(HeaderNames.WWWAuthenticate))
        {
            context.Response.Headers[HeaderNames.WWWAuthenticate] = "Basic";
        }

        context.Response.Headers[HeaderNames.Authorization] = "";
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    }
}