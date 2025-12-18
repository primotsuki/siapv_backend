namespace siapv_backend.Models.DTOResponses
{
    public class DTOSolicitudViaje
    {
        public int Id { get; set; }
        public DateOnly fechaInicio { get; set; }
        public DateOnly fechaFin { get; set; }
        public string destino { get; set; } = null!;
        public string proyecto { get; set; } = null!;
        public string estado { get; set; } = null!;
        public int estadoId { get; set; }
    }
}