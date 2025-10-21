

using System.ComponentModel.DataAnnotations.Schema;

namespace puc.Models
{
    #pragma warning disable CS1591
    [Table("VeiculoUsuarios")]
    public class VeiculoUsuarios
    {
        public int VeiculoId { get; set; }
        public Veiculo? Veiculo { get; set; }

        
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

    }
}