using System.ComponentModel.DataAnnotations;
using puc.Models;

namespace puc.DTOS
{
    #pragma warning disable CS1591
    public class UsuarioDto
    {

        public int? UsuarioId { get; set; }

        [Required]
        public string? Nome { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public Perfil Perfil { get; set; }

    }
}