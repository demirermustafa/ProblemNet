using Microsoft.AspNetCore.Builder;

namespace ProblemNet.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseProblemDetailsExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ProblemDetailsMiddleware>();
        }
    }
}
