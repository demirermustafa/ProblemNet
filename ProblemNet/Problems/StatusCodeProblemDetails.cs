using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace ProblemNet.Problems
{
    public class StatusCodeProblemDetails : ProblemDetails
    {
        public StatusCodeProblemDetails(int statusCode)
        {
            Status = statusCode;
            Type = $"https://httpstatuses.com/{statusCode}";
            Title = ReasonPhrases.GetReasonPhrase(statusCode);
        }
    }
}