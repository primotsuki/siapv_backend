namespace siapv_backend.Models
{
    public interface IEmailService
    {
        Task<Boolean> sendInicioViaje(SolicitudViaje solicitud);
    }
}