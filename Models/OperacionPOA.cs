using System.ComponentModel.DataAnnotations.Schema;

namespace siapv_backend.Models
{
    public class OperacionPOA
    {
        public int Id {get; set;}
        [Column(TypeName ="varchar(15)")]
        public string codigo_op {get; set;} = null!;
        [Column(TypeName ="varchar(200)")]
        public string nombre_operacion {get; set;} = null!;
        public string prog {get; set;} = null!;
        public string proyecto {get; set;} = null!;
        public string fuente {get; set;} = null!;
        public Boolean activo { get; set;}
    }
}