using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using puc.DTOS;

namespace puc.Models
{
#pragma warning disable CS1591
    [Table("Usuarios")]
    public class Usuario 
    {
        [Key]
        public int UsuarioId { get; set; }
        [Required]
        public string? Nome { get; set; }

        [Required]
        [JsonIgnore] //nao mostrar senha
        public string? Password { get; set; }
        [Required]
        public Perfil Perfil { get; set; }

        //usuairo tem uma coleção de veiculos
        [JsonIgnore]
        public ICollection<VeiculoUsuarios>? Veiculos { get; set; }
    }

    public enum Perfil
    {
        [Display(Name = "Administrador")]
        Administrador,

        [Display(Name = "Usuário")]
        Usuario
    }
}
