using System.Text;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Back.Middleware.SpaIndex;

public class SpaIndexMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SpaIndexMiddleware> _logger;

    public SpaIndexMiddleware(
        RequestDelegate next,
        ILogger<SpaIndexMiddleware> logger
    )
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.Path.Value;

        if (!String.IsNullOrEmpty(path) && !path.StartsWith("/api") && !Path.HasExtension(path))
        {

            var file = new FileInfo(@"wwwroot/index.html");
            var jsonString = JsonSerializer.Serialize(
                new
                {
                    appVersion = "0.0.1",
                    appName = "CareWell"
                }
            );

            if (file.Exists)
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                context.Response.ContentType = "text/html";
                var html = await File.ReadAllTextAsync(file.FullName);
                html = html.Replace(
                    "<!-- config -->",
                    $@"<script>window.appSettings = {jsonString};</script>"
                );
                await context.Response.WriteAsync(html, Encoding.UTF8);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Unable to find the Index file", Encoding.UTF8);
            }
        }
        else
            await _next(context);
    }
}
