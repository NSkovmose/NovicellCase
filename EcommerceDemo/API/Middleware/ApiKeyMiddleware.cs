using Microsoft.Identity.Client;

namespace PicoPlanner.Service.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEY = "XApiKey";
        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // Allow SwaggerUI to be accessed without an API key, still requires key to be set in SwaggerUI
            if (!context.Request.Path.StartsWithSegments("/swagger"))
            {

                if (!context.Request.Headers.TryGetValue(APIKEY, out
                        var extractedApiKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Api Key was not provided ");
                    return;
                }
                var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
                var apiKey = appSettings.GetValue<string>(APIKEY);
                if (!apiKey.Equals(extractedApiKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized client");
                    return;
                }
            }
            await _next(context);
        }
    }
}
