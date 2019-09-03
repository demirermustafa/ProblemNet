using System.ComponentModel.DataAnnotations;

namespace ProblemsApi.Controllers
{
    public class UserCreateRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}