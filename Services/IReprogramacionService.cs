using siapv_backend.Models;
using siapv_backend.Models.DTORequests;

namespace siapv_backend.Services
{
    public interface IReprogramacionService
    {
        Task<Reprogramacion> createReprogramacion(DTOReprogramacion request);
        Task<Byte[]> getFormularioReprogramacion(int solicitudId);
    }
}