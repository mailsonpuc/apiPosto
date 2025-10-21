
using System.ComponentModel.DataAnnotations;

namespace puc.DTOS
{
    public class AuthenticateDto
    {
        [Required]
        public int AuthenticateDtoId { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}