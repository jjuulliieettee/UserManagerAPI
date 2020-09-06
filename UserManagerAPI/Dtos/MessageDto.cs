using System.ComponentModel.DataAnnotations;

namespace UserManagerAPI.Dtos
{
    public class MessageDto
    {
        [Required]
        public string Message { get; set; }
    }
}
