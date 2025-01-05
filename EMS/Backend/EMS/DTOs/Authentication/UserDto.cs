using System.ComponentModel.DataAnnotations;

namespace EMS.DTOs.Authentication
{
    public class UserDto
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; } = null!;
    }
}
