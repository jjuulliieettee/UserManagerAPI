using System.ComponentModel.DataAnnotations;

namespace UserManagerAPI.Dtos
{
    public class UserUpdateDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength( 25 )]
        public string Username { get; set; }

        [Required]
        [MaxLength( 250 )]
        public string Name { get; set; }

        public string Password { get; set; }
    }
}
