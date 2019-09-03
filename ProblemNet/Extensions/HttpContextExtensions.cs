using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace ProblemNet.Extensions
{
    public static class HttpContextExtensions
    {
        private static readonly RouteData EmptyRouteData = new RouteData();

        private static readonly ActionDescriptor EmptyActionDescriptor = new ActionDescriptor();

        private static readonly HashSet<string> AllowedHeaderNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                                                                     {
                                                                             HeaderNames.AccessControlAllowCredentials,
                                                                             HeaderNames.AccessControlAllowHeaders,
                                                                             HeaderNames.AccessControlAllowMethods,
                                                                             HeaderNames.AccessControlAllowOrigin,
                                                                             HeaderNames.AccessControlExposeHeaders,
                                                                             HeaderNames.AccessControlMaxAge,
                                                                             HeaderNames.StrictTransportSecurity,
                                                                             HeaderNames.WWWAuthenticate,
                                                                     };

        public static Task WriteProblemDetailsAsync(this HttpContext context, ProblemDetails details)
        {
            if (String.IsNullOrWhiteSpace(details.Instance))
            {
                details.Instance = context.Request.Path;
            }

            var result = new ObjectResult(details)
                         {
                                 StatusCode = details.Status ?? context.Response.StatusCode,
                                 DeclaredType = details.GetType(),
                         };

            result.ContentTypes.Add("application/problem+json");
            result.ContentTypes.Add("application/problem+xml");

            return context.ExecuteResultAsync(result);
        }

        public static void ClearResponse(this HttpContext context, int statusCode)
        {
            var headers = new HeaderDictionary();

            headers.Append(HeaderNames.CacheControl, "no-cache, no-store, must-revalidate");
            headers.Append(HeaderNames.Pragma, "no-cache");
            headers.Append(HeaderNames.Expires, "0");

            foreach (KeyValuePair<string, StringValues> header in context.Response.Headers)
            {
                //Copy over the existing Access-Control-* headers after resetting the response.
                if (AllowedHeaderNames.Contains(header.Key))
                {
                    headers.Add(header);
                }
            }

            context.Response.Clear();
            context.Response.StatusCode = statusCode;

            foreach (KeyValuePair<string, StringValues> header in headers)
            {
                context.Response.Headers.Add(header);
            }
        }

        private static Task ExecuteResultAsync<TResult>(this HttpContext context, TResult result)
                where TResult : IActionResult
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (result == null) throw new ArgumentNullException(nameof(result));

            var executor = context.RequestServices.GetRequiredService<IActionResultExecutor<TResult>>();

            RouteData routeData = context.GetRouteData() ?? EmptyRouteData;
            var actionContext = new ActionContext(context, routeData, EmptyActionDescriptor);

            return executor.ExecuteAsync(actionContext, result);
        }
    }
}
