

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using puc.DTOS;

namespace puc.Models
{

    // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591
    [Table("Veiculos")]
    public class Veiculo : LinksHATEOAS
    {
        [Key]
        public int VeiculoId { get; set; }
        [Required]
        public string? Marca { get; set; }
        [Required]
        public string? Modelo { get; set; }
        [Required]
        public string? Placa { get; set; }
        [Required]
        public int AnoFabricacao { get; set; }
        [Required]
        public int AnoModelo { get; set; }

        /*
        Veiculo tem muitos consumos
        vaiulo tem uma colecao de consumos

        consumo tem apenas um veiculo

        È um relacionamento de 1:N
        */
        // [JsonIgnore]
        public ICollection<Consumo>? Consumos { get; set; }
        // [JsonIgnore]
        public ICollection<VeiculoUsuarios>? Usuarios { get; set; }

    }
}