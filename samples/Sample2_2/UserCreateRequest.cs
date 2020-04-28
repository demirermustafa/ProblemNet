using System.ComponentModel.DataAnnotations;

namespace Sample2_2
{
    public class UserCreateRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}