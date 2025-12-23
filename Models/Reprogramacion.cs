namespace siapv_backend.Models
{
    public class Reprogramacion
    {
        public int Id { get; set; }
        public int solicitudId { get; set; }
        public SolicitudViaje? solicitud {get; set;}
        public string linea { get; set; } = null!;
        public DateOnly fecha_emsision { get; set; }
        public int origenId { get; set; }
        public LugarDestino? origen { get; set; }
        public int destinoId { get; set; }
        public LugarDestino? destino { get; set; }
        public string nro_boleto { get; set; } = null!;
        public double importe { get; set; }
        public string justificacion {get; set; } = null!;
        public DateTime createAt { get; set; }

    }
}