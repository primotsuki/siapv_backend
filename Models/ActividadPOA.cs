using System.ComponentModel.DataAnnotations.Schema;

namespace siapv_backend.Models
{
    public class ActividadPOA
    {
        public int Id {get; set;}
        [Column(TypeName = "varchar(30)")]
        public string codigo { get; set;} =null!;
        [Column(TypeName = "varchar(10)")]
        public string tipo {get; set;} =null!;
        public string descripcion {get; set;} =null!;
        public Boolean activo {get; set;}
    }
}