using System.ComponentModel.DataAnnotations.Schema;

namespace siapv_backend.Models
{
    public class FuenteFinanciamiento
    {
        public int Id { get; set; }
        [Column(TypeName ="varchar(10)")]
        public required string descripcion {get; set;}
        public int codigo_fuente {get; set;}
        [Column(TypeName = "varchar(50)")]
        public required string literal {get; set;}
    }
}