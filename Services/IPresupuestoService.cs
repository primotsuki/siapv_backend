using siapv_backend.Models;
using siapv_backend.Models.DTORequests;
namespace siapv_backend.Services
{
    public interface IPresupuestoService
    {
        Task<List<CategoriaProgramatica>> getCategorias();
        Task<CertificacionPresupuestaria> createCertificacion(DTOCertificacionPres request);

    }
}