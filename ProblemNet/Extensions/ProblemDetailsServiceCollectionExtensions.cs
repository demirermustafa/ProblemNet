using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using ProblemNet.Options;

namespace ProblemNet.Extensions
{
    public static class ProblemDetailsServiceCollectionExtensions
    {
        public static void AddProblemDetails(this IServiceCollection services,
                                             Action<ProblemDetailsOptions> setupAction = null)
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<ProblemDetailsOptions>, ProblemDetailsOptionsConfigurator>());
            if (setupAction != null)
                services.ConfigureProblemDetailsOptions(setupAction);
        }

        public static void ConfigureProblemDetailsOptions(
                this IServiceCollection services,
                Action<ProblemDetailsOptions> setupAction)
        {
            services.Configure<ProblemDetailsOptions>(setupAction);
        }
    }
}
