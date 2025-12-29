namespace siapv_backend.Models.DTOResponses
{
    public class DTOSolicitudesPendientes
    {
        public int solicitudId{get; set;}
        public string destino {get; set;} = null!;
        public DateOnly fechaInicio {get; set;} 
        public DateOnly fechaFin {get; set;}
        public int estadoId {get; set;}
        public string estado {get;  set;} = null!;
    }


}