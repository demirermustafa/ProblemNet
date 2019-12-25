using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProblemNet.Exceptions;
using ProblemNet.Extensions;
using ProblemNet.Options;
using static System.String;

namespace ProblemNet
{
    public class ProblemDetailsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ProblemDetailsMiddleware> _logger;
        private readonly ProblemDetailsOptions _options;

        public ProblemDetailsMiddleware(RequestDelegate next,
                                        ILogger<ProblemDetailsMiddleware> logger,
                                        IOptions<ProblemDetailsOptions> options)
        {
            _next = next;
            _logger = logger;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the problem details middleware will not be executed.");
                    throw;
                }

                Log(exception);

                context.ClearResponse(StatusCodes.Status500InternalServerError);

                if (exception is ProblemDetailsException problem)
                {
                    if (!IsNullOrWhiteSpace(_options.DefaultTypeBaseUri))
                    {
                        problem.Type = $"{_options.DefaultTypeBaseUri.TrimEnd('/')}/{problem.Type}";
                    }

                    await context.WriteProblemDetailsAsync(problem.ProblemDetails());
                    return;
                }

                var internalServerException = new InternalServerException(exception, _options.DisplayUnhandledExceptionDetails(context));
                await context.WriteProblemDetailsAsync(internalServerException);

            }
        }

        private void Log(Exception exception)
        {
            if (exception is ProblemDetailsException problemDetailsException)
            {
                if (problemDetailsException.Status < (int)HttpStatusCode.InternalServerError && problemDetailsException.Status >= (int)HttpStatusCode.BadRequest)
                {
                    _logger.LogInformation("{@ProblemDetails}", problemDetailsException.ProblemDetails());
                }
                else
                {
                    _logger.LogError(exception, "{@ProblemDetails}", problemDetailsException.ProblemDetails());
                }
            }
            else
            {
                _logger.LogError(exception, exception.Message);
            }
        }
    }
}
