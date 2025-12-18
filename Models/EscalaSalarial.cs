using NpgsqlTypes;
using System.Text.Json.Serialization;

namespace siapv_backend.Models
{
    public class EscalaSalarial
    {
        public int Id { get; set; }
        public int nivelSalarial { get; set; }
        public string denominacion { get; set; } = null!;
        public string haber_mensual { get; set; } = null!;
        public int ResolucionEscalaId { get; set; }
        public Boolean activo { get; set; }
    }
}