namespace siapv_backend.Models
{
    public class RevisionFormularios
    {
        public int Id {get; set;}
        public int solicitudId { get; set; }
        public SolicitudViaje? solicitud {get; set;}
        public bool fucav { get; set;}
        public bool poa {get; set;}
        public bool presupuesto {get; set;}
        public bool memo {get; set;}
        public bool informe {get; set;}
        public int estadoId {get; set;}
        public EstadoSolicitud? estado {get; set;}
        public DateTime createdAt {get; set;}
    }
}