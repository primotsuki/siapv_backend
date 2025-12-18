namespace siapv_backend.Models
{
    public class CertificacionPresupuestaria
    {
        public int Id {get; set;}
        public int gestion {get; set;}
        public int correlativo {get; set;}
        public int solicitudId {get; set;}
        public SolicitudViaje? solicitud {get; set;}
        public int categoriaId {get; set;}
        public CategoriaProgramatica? categoria {get; set;}
        public double importe_solicitado {get; set;}
        public double saldo_categoria {get; set;}
        public string concepto {get; set;} = null!;
        public DateTime createdAt {get; set;}
        public DateTime updatedAt {get; set;}
    }
}