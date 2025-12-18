using siapv_backend.Models;

namespace siapv_backend.Services
{
    public interface IFinanciamientoService
    {
        Task<List<FuenteFinanciamiento>> getFinanciamientoList();
    }
}