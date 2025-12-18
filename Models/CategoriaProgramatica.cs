using System.ComponentModel.DataAnnotations.Schema;

namespace siapv_backend.Models
{
    public class CategoriaProgramatica
    {
        public int Id {get; set;}
        [Column(TypeName = "varchar(50)")]
        public string categoria {get; set;} = null!;
        public string descripcion_categoria {get; set;} = null!;
        public int partida {get; set;}
        public double presupuesto_vigente {get; set;}
        public double saldo { get; set; }
        public Boolean activo { get; set; }
    }
}