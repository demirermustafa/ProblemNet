using Microsoft.AspNetCore.Mvc;

namespace Sample3_1
{
    public class UserNotFoundProblem : ProblemDetails
    {
        public int UserId { get; set; }
    }
}
