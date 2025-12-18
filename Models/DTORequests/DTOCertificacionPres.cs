namespace siapv_backend.Models.DTORequests
{
    public class DTOCertificacionPres
    {
        public int solicitudId {get; set;}
        public int categoriaId {get; set;}
        public string concepto {get; set;} = null!;
    }
}