using System.ComponentModel.DataAnnotations.Schema;

namespace siapv_backend.Models
{
    public class TipoViaje
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string tipo { get; set; } = null!;
        public double monto_viatico {get; set; }
    }
}