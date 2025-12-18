using System.ComponentModel.DataAnnotations.Schema;

namespace siapv_backend.Models
{
    public class SolicitudViaje
    {
        public int Id { get; set; }
        public DateOnly fechaInicio { get; set; }
        public DateOnly fechaFin { get; set; }
        public TimeOnly? horaInicio { get; set; }
        public TimeOnly? horaFin { get; set; }
        public string descripcion_viaje { get; set; } = null!;
        [Column(TypeName = "varchar(30)")]
        public string cite_memo { get; set; } = null!;
        public int? citeId { get; set; }
        public DocGenerado? cite { get; set; }
        public int proyectoId { get; set; }
        public Proyecto? proyecto { get; set; }
        public int empleadoId {get; set; }
        public int designadorId { get; set; }
        public int? lugarOrigenId { get; set; }
        public LugarDestino? lugarOrigen { get; set; }
        public int lugarDestinoId { get; set; }
        public LugarDestino? lugarDestino { get; set; }
        public int fuenteId { get; set; }
        public FuenteFinanciamiento? fuente { get; set; }
        public int? transporteId { get; set; }
        public MediosTransporte? transporte { get; set; }
        public int? tipoViajeId { get; set; }
        public TipoViaje? tipoViaje { get; set; }
        public int? estadoId { get; set; }
        public EstadoSolicitud? estado { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}