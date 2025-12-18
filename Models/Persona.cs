using NpgsqlTypes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace siapv_backend.Models
{
    public class Persona
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(50)")]
        public required string Nombres { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string apellido_paterno { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string apellido_materno { get; set; } = null!;
        [Column(TypeName = "varchar(20)")]
        public string carnet { get; set; } = null!;
        [Column(TypeName = "varchar(5)")]
        public string? complemento_carnet { get; set; } = null!;
        public DateOnly? fecha_nacimiento { get; set; }
        public char? sexo { get; set; }
        public int? celular { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string? localidad {get; set; }
        [Column(TypeName = "varchar(200)")]
        public string? direccion {get; set; }

        public string? email_personal { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? nro_libreta_militar {  get; set; }
        [Column(TypeName = "varchar(100)")]
        public string? persona_referencia { get;set; }
        [Column(TypeName = "varchar(30)")]
        public string? parentezco { get; set; }
        [Column (TypeName = "varchar(20)")]
        public string? nro_cel_referencia { get;set; }
        [DefaultValue(false)]
        public Boolean completed { get; set; } = false;
        public NpgsqlTsVector SearchVector { get; set; } = null!;
        public Usuario? Usuario { get; set; }
    }
}
