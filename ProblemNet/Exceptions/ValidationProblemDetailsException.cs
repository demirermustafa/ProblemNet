using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ProblemNet.Problems;

namespace ProblemNet.Exceptions
{
    public class ValidationProblemDetailsException : ProblemDetailsException
    {
        private ValidationProblem[] _errors { get; }

        public ValidationProblemDetailsException(params ValidationProblem[] validationProblems)
        {
            _errors = validationProblems ?? throw new ArgumentNullException(nameof(validationProblems));
            Status = StatusCodes.Status400BadRequest;
            Type = $"https://httpstatuses.com/400";
            Detail = "Model State Validation";
        }

        public ValidationProblemDetailsException(string field, params string[] messages)
                : this(new ValidationProblem(field, messages))
        {
        }

        public override ProblemDetails ProblemDetails()
        {
            var problemDetails = new ModelStateValidationProblemDetails()
                                 {
                                         Title = Title,
                                         Detail = Detail,
                                         Instance = Instance,
                                         Status = Status ?? StatusCodes.Status400BadRequest,
                                         Type = Type,
                                         Errors = _errors.ToList()
                                 };

            foreach (KeyValuePair<string, object> extension in Extensions)
            {
                problemDetails.Extensions.Add(extension);
            }

            return problemDetails;
        }
    }
}
