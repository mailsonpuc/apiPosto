
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using puc.DTOS;

namespace puc.Models
{
    #pragma warning disable CS1591
    [Table("Consumos")]
    public class Consumo : LinksHATEOAS
    {
        [Key]
        public int ConsumoId { get; set; }
        [Required]
        public string? Descricao { get; set; }
        [Required]
        public DateTime Data { get; set; }
        [Required]
        public decimal Valor { get; set; }
        [Required]
        public TipoCombustivel Tipo { get; set; }


        //forenkey
        public int VeiculoId { get; set; }
        //navegação virtual, quando o ef for carregar consumo, vai carregar tambem Veiculo
        [JsonIgnore]
        public Veiculo? Veiculo { get; set; }

    }

    public enum TipoCombustivel
    {
        Diesel,
        Etanol,
        Gasolina
    }
}