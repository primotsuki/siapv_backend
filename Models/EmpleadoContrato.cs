using NpgsqlTypes;
using System.ComponentModel.DataAnnotations.Schema;

namespace siapv_backend.Models
{
    public class EmpleadosContrato
    {
        public int Id { get; set; }
        [Column(TypeName ="varchar(200)")]
        public string DenominacionCargo { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string nro_contrato { get; set; } = null!;
        public DateOnly fechaInicio { get; set; }
        public DateOnly? fechaFinContrato { get; set; }
        public DateOnly? fechaFin {  get; set; }
        public int DependenciaId { get; set; }
        public Dependencia? Dependencia { get; set; }
        public int? InmediatoSuperiorId { get; set; }
        public virtual EmpleadosContrato? InmediatoSuperior { get; set; }
        public int escalaSalarialId { get; set; }
        public EscalaSalarial? EscalaSalarial { get; set; }
        public int personaId { get; set; }
        public Persona? Persona { get; set; }
        public Boolean activo { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public NpgsqlTsVector searchCargo { get; set; } = null!;
        public DateOnly? fecha_baja {get; set;}  
    }
}
