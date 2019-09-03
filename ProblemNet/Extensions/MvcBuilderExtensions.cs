using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using ProblemNet.Problems;

namespace ProblemNet.Extensions
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder UseProblemDetailsApiBehavior(this IMvcBuilder builder)
        {
            builder.Services.Configure<ApiBehaviorOptions>(options =>
                                                               {
                                                                   options.InvalidModelStateResponseFactory = context =>
                                                                                                                  {
                                                                                                                      var problemDetails = new ModelStateValidationProblemDetails(context.ModelState)
                                                                                                                                           {
                                                                                                                                                   Instance = context.HttpContext.Request.Path,
                                                                                                                                                   Status = StatusCodes.Status400BadRequest,
                                                                                                                                                   Type = $"https://httpstatuses.com/400",
                                                                                                                                                   Detail = "Model State Validation"
                                                                                                                                           };

                                                                                                                      return new BadRequestObjectResult(problemDetails)
                                                                                                                             {
                                                                                                                                     ContentTypes =
                                                                                                                                     {
                                                                                                                                             "application/problem+json",
                                                                                                                                             "application/problem+xml"
                                                                                                                                     }
                                                                                                                             };
                                                                                                                  };
                                                               });
            return builder;
        }
    }
}
