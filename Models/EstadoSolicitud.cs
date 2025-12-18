using System.ComponentModel.DataAnnotations.Schema;

namespace siapv_backend.Models
{
    public class EstadoSolicitud
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string estado { get; set; } = null!;
    }
}