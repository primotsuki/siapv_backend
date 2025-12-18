namespace siapv_backend.Models.DTOResponses
{
    public class DTOEmpleadoSolicitud
    {
        public int Id { get; set; }
        public string nombres { get; set; } = null!;
        public string apellido_paterno { get; set; } = null!;
        public string apellido_materno { get; set; } = null!;
        public int empleadoId { get; set; }
        public DateOnly fechaInicio { get; set; }
        public DateOnly fechaFin { get; set; }
        public string destino { get; set; } = null!;
        public string proyecto { get; set; } = null!;
        public string estado { get; set; } = null!;
        public int estadoId { get; set; }
    }
}