using NpgsqlTypes;
using System.Text.Json.Serialization;

namespace siapv_backend.Models.DTOResponses
{
    public class DTOEmpleadoInfo
    {
        public int empleadoId {get; set;}
        public string nombres {get; set;} = null!;
        public string apellido_paterno { get; set; } = null!;
        public string apellido_materno { get; set; } = null!;
        public string carnet { get; set; } = null!;
        public string denominacionCargo { get; set; } = null!;
        [JsonIgnore]
        public NpgsqlTsVector searchPersona { get; set; } = null!;
        [JsonIgnore]
        public NpgsqlTsVector searchCargo { get; set; } = null!;
    }
}