using System;
using Microsoft.AspNetCore.Http;

namespace ProblemNet.Options
{
    public class ProblemDetailsOptions
    {
        public string DefaultTypeBaseUri { get; set; }

        public Func<HttpContext, bool> DisplayUnhandledExceptionDetails { get; set; }
    }
}
