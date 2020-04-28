using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProblemNet.Exceptions;
using ProblemNet.Problems;

namespace Sample3_1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SamplesController : ControllerBase
    {
        [HttpGet("problem-details-simple")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<string>> ThrowProblemDetailsSimpleException()
        {
            throw new ProblemDetailsException()
            {
                Type = "user-not-exists",
                Title = "User Not Exists",
                Status = (int)HttpStatusCode.NotFound,
                Detail = "Users cannot be found"
            };
        }

        [HttpGet("problem-details-additional-members")]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public ActionResult<string> ThrowProblemDetailsExceptionWithAdditionalMembers()
        {
            throw new ProblemDetailsException
            {
                Status = (int)HttpStatusCode.NotFound,
                Title = $"Users cannot be found",
                Extensions =
                          {
                                  new KeyValuePair<string, object>("Order", new { OrderId = 1, OrderDate = DateTime.UtcNow, Status = "Approved" }),
                                  new KeyValuePair<string, object>("UserId", 123)
                          }
            };
        }

        [HttpGet("unhandled-exception")]
        [ProducesResponseType(500)]
        public void ThrowNullRefenceException()
        {
            throw new NullReferenceException();
        }

        [HttpGet("custom-validation-single-field")]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public ActionResult<string> ThrowCustomValidationSingleField()
        {
            throw new ValidationProblemDetailsException("Email", "Email address length must be greater than 5 characters",
                                                        "Email address must be email format")
            {
                Detail = "Error while creating account. Please see errors for detail.",
                Status = (int)HttpStatusCode.BadRequest,
                Title = "Register Error"
            };
        }

        [HttpGet("custom-validation-multi-field")]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public ActionResult<string> ThrowCustomValidationMultiField()
        {
            var validationProblems = new List<ValidationProblem>
                                     {
                                             new ValidationProblem("FirstName", "FirstName length must be less than 50 characters", "FirstName should only contain letters"),
                                             new ValidationProblem("Email", "Email address length must be greater than 5 characters", "Email address must be email format")
                                     };

            throw new ValidationProblemDetailsException(validationProblems.ToArray())
            {
                Detail = "Error while creating account. Please see errors for detail.",
                Status = (int)HttpStatusCode.BadRequest,
                Title = "Register Error"
            };
        }

        [HttpPost("model-state-validation")]
        public IActionResult CreateUser([FromBody] UserCreateRequest request)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            return Created("", request);
        }
    }
}
