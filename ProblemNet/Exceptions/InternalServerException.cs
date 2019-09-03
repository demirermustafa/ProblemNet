using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using ProblemNet.Problems;

namespace ProblemNet.Exceptions
{
    public class InternalServerException : StatusCodeProblemDetails
    {
        public InternalServerException(Exception error, bool displayExceptionDetail = false)
                :
                this(error, StatusCodes.Status500InternalServerError, displayExceptionDetail: displayExceptionDetail)
        {
        }

        private InternalServerException(Exception error, int statusCode, bool displayExceptionDetail = false)
                : base(statusCode)
        {
            Exception = displayExceptionDetail ? error : null ;
            Detail = "An error occurred while processing your request";
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "exception")]
        public Exception Exception { get; }
    }
}
