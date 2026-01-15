namespace siapv_backend.Models.DTORequests
{
    public class DTOInformeViaje
    {
        public int solicitudId {get; set;}
        public string cite_doc {get; set;} = null!;
        public SolicitudViaje? solicitud {get; set;}
        public string antecedentes {get; set;} = null!;
        public string desarrollo {get; set;} = null!;
        public string conclusion {get; set;} = null!;
    }
}