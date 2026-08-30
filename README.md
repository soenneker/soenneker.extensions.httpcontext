[![](https://img.shields.io/nuget/v/Soenneker.Extensions.HttpContext.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Extensions.HttpContext/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.httpcontext/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.httpcontext/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Extensions.HttpContext.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Extensions.HttpContext/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.httpcontext/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.httpcontext/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.HttpContext
Small ASP.NET Core helpers for local-request checks, Basic authentication challenges, and proxy-aware client IP lookup.

## Installation

```bash
dotnet add package Soenneker.Extensions.HttpContext
```

## Identify local requests

```csharp
using Soenneker.Extensions.HttpContext;

if (httpContext.IsLocalRequest())
{
    // Apply behavior intended only for requests from this host.
}
```

`IsLocalRequest()` returns `true` when the remote address is loopback, matches the local connection address, or both addresses are absent. It returns `false` when only the remote address is absent. It examines the connection addresses after any middleware that may have rewritten them.

## Return a Basic authentication challenge

```csharp
httpContext.SetUnauthorized();
return;
```

`SetUnauthorized()` sets status code `401` and adds `WWW-Authenticate: Basic` unless a challenge header already exists. It does not write a response body or end request processing, so return from the endpoint or middleware after calling it.

## Read the client IP

```csharp
string? clientIp = httpContext.GetRequestIp();
```

The lookup order is:

1. A valid IP address in `CF-Connecting-IP`.
2. The first address in `X-Forwarded-For`, when it is a valid IP address.
3. `HttpContext.Connection.RemoteIpAddress`.

Forwarding headers are supplied by the caller and can be spoofed unless your edge proxy removes incoming copies and writes trusted values. Only use the returned forwarded address for authorization, rate limiting, or auditing when that trust boundary is enforced. In standard ASP.NET Core deployments, prefer configuring Forwarded Headers Middleware and then reading `RemoteIpAddress` directly when possible.

Malformed or blank forwarding values are ignored. If no usable address is available, `GetRequestIp()` returns `null`.
