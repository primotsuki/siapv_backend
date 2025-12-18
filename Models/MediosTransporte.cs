using System.ComponentModel.DataAnnotations.Schema;

namespace siapv_backend.Models
{
    public class MediosTransporte
    {
        public int Id { get; set; }
        [Column(TypeName ="varchar(40)")]
        public required string transporte { get; set; }
    }
}