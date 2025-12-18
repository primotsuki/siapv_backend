namespace siapv_backend.Models.DTORequests
{
    public class DTOFucav
    {
        public int Id { get; set; }
        public int lugarOrigenId { get; set; }
        public int transporteId { get; set; }
        public int tipoviajeId { get; set; }
        public TimeOnly horaInicio { get; set; }
        public TimeOnly horaFin { get; set; }
    }
}