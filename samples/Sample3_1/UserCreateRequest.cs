using System.ComponentModel.DataAnnotations;

namespace Sample3_1
{
    public class UserCreateRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}