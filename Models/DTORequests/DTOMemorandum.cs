namespace siapv_backend.Models.DTORequests
{
    public class DTOMemorandum
    {
        public DateOnly fechaInicio { get; set; }
        public DateOnly fechaFin { get; set; }
        public string descripcion_viaje { get; set; } =null!;
        public string cite_memo { get; set; } = null!;
        public int proyectoId { get; set; }
        public int lugarDestinoId { get; set; }
        public int fuenteId { get; set; }
        public int empleadoId {get; set; }
    }
}