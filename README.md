[![](https://img.shields.io/nuget/v/Soenneker.Extensions.HttpContext.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Extensions.HttpContext/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.httpcontext/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.httpcontext/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Extensions.HttpContext.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Extensions.HttpContext/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.httpcontext/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.httpcontext/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.HttpContext
A collection of helpful HttpContext extension methods.

## Installation

```bash
dotnet add package Soenneker.Extensions.HttpContext
```

## Quick start

```csharp
using Soenneker.Extensions.HttpContext;

// Given an existing Microsoft.AspNetCore.Http.HttpContext named context:
var result = context.IsLocalRequest();
```

## Common operations

- `IsLocalRequest()` - Determines whether the request is coming from a local address.
- `SetUnauthorized()` - Sets the response to indicate that the request is unauthorized.
- `GetRequestIp()` - Retrieves the real client IP from Cloudflare or standard proxy headers.
