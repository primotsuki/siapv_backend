namespace siapv_backend.Models
{
    public class CertificacionPOA
    {
        public int Id {get; set;}
        public int gestion {get; set;}
        public int correlativo {get; set;}
        public int solicitudId {get; set;}
        public SolicitudViaje? solicitud {get; set;}
        public int actividadAmpId { get; set; }
        public ActividadPOA? actividadAmp { get; set; }
        public int actividadAcpId { get; set; }
        public ActividadPOA? actividadAcp { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}