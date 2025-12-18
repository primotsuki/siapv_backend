using System.ComponentModel.DataAnnotations.Schema;

namespace siapv_backend.Models
{
    public class Proyecto
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string descripcion { get; set; } = null!;
    }
}