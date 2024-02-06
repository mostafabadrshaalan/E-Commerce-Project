using API.ResponseModule;
using System.Net;
using System.Text.Json;

namespace API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly RequestDelegate next;
        private readonly IHostEnvironment environment;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger,
                                   RequestDelegate next,
                                   IHostEnvironment environment)
        {
            this.logger = logger;
            this.next = next;
            this.environment = environment;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                httpContext.Response.ContentType="application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = environment.IsDevelopment()
                             ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                             : new ApiException((int)HttpStatusCode.InternalServerError);
                var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response, option);
                await httpContext.Response.WriteAsync(json);
            }
        }
    }
}
