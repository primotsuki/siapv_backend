using System.ComponentModel.DataAnnotations.Schema;

namespace siapv_backend.Models
{
    public class LugarDestino
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(60)")]
        public string destino { get; set; } = "";
    }
}