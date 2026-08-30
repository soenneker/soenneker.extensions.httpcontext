using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Threading.Tasks;
using Soenneker.Tests.Unit;

namespace Soenneker.Extensions.HttpContext.Tests;

public class HttpContextExtensionTests : UnitTest
{
    [Test]
    public async Task SetUnauthorized_does_not_add_an_authorization_response_header()
    {
        var context = new DefaultHttpContext();

        context.SetUnauthorized();

        await Assert.That(context.Response.StatusCode).IsEqualTo(StatusCodes.Status401Unauthorized);
        await Assert.That(context.Response.Headers[HeaderNames.WWWAuthenticate].ToString()).IsEqualTo("Basic");
        await Assert.That(context.Response.Headers.ContainsKey(HeaderNames.Authorization)).IsFalse();
    }

    [Test]
    public async Task GetRequestIp_ignores_malformed_forwarding_headers()
    {
        var context = new DefaultHttpContext();
        context.Connection.RemoteIpAddress = IPAddress.Loopback;
        context.Request.Headers["CF-Connecting-IP"] = "not-an-ip";
        context.Request.Headers["X-Forwarded-For"] = "also-not-an-ip";

        await Assert.That(context.GetRequestIp()).IsEqualTo(IPAddress.Loopback.ToString());
    }

    [Test]
    public async Task GetRequestIp_returns_the_first_forwarded_address()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers["X-Forwarded-For"] = "203.0.113.10, 10.0.0.4";

        await Assert.That(context.GetRequestIp()).IsEqualTo("203.0.113.10");
    }
}
