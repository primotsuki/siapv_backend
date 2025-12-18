using NpgsqlTypes;
using System.Text.Json.Serialization;
namespace siapv_backend.Models.DTOResponses
{
    public class DTOUserInfo
    {
        public int userId {get; set;}
        public int personaId {get; set;}
        public int empleadoId {get; set;}
        public  string nombres {get; set;} =null!;
        public string apellido_paterno {get; set;} = null!;
        public string apellido_materno { get; set;} = null!;
        public string cargo {get; set;} =null!;
        public int? roleId {get; set;}
        public string? role {get; set;} = null!;
        [JsonIgnore]
        public NpgsqlTsVector SearchVector { get; set; } = null!;
    }
}