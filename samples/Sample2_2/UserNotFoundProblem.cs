using Microsoft.AspNetCore.Mvc;

namespace Sample2_2
{
    public class UserNotFoundProblem : ProblemDetails
    {
        public int UserId { get; set; }
    }
}
