using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
#if NETSTANDARD2_0
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
#else
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;
#endif
namespace ProblemNet.Options
{
    public class ProblemDetailsOptionsConfigurator : IConfigureOptions<ProblemDetailsOptions>
    {
        public void Configure(ProblemDetailsOptions options)
        {
            if (options.DisplayUnhandledExceptionDetails == null)
            {
                options.DisplayUnhandledExceptionDetails = DisplayUnhandledExceptionDetails;
            }
        }

        private static bool DisplayUnhandledExceptionDetails(HttpContext context)
        {
#if NETSTANDARD2_0
            return context.RequestServices.GetRequiredService<IHostingEnvironment>().IsDevelopment();

#else
            return context.RequestServices.GetRequiredService<IHostingEnvironment>().IsEnvironment(Environments.Development);
#endif
        }
    }
}
