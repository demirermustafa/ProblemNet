using Microsoft.AspNetCore.Mvc;

namespace ProblemsApi
{
    public class UserNotFoundProblem : ProblemDetails
    {
        public int UserId { get; set; }
    }
}
