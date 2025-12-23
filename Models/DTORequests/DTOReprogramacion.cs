namespace siapv_backend.Models.DTORequests
{
    public class DTOReprogramacion
    {
        public int solicitudId { get; set; }
        public string linea { get; set; } = null!;
        public DateOnly fecha_emision { get; set; }
        public int origenId { get; set; }
        public int destinoId { get; set; }
        public string nro_boleto { get; set; } = null!;
        public double importe { get; set; }
        public string justificacion {get; set; } = null!;
    }
}