using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace siapv_backend.Models
{
    public class Dependencia
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(100)")]
        public required string descripcion { get; set; }
        [Column(TypeName = "varchar(30)")]
        public required string sigla { get; set; }
        public int? dependenciaId { get; set; }
        public Boolean activo { get; set; }
        [JsonIgnore]
        public DateTime createAt { get; set; }
        [JsonIgnore]
        public DateTime updateAt { get; set; }

    }
}