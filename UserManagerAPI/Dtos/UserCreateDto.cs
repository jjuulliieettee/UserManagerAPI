using System.ComponentModel.DataAnnotations;

namespace UserManagerAPI.Dtos
{
    public class UserCreateDto
    {
        [Required]
        [MaxLength( 25 )]
        public string Username { get; set; }

        [Required]
        [MaxLength( 250 )]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
