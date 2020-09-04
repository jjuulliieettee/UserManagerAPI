using System.ComponentModel.DataAnnotations;

namespace UserManagerAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

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
