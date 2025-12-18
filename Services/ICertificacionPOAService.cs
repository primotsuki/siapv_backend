using siapv_backend.Models;
using siapv_backend.Models.DTORequests;
namespace siapv_backend.Services
{
    public interface ICertificacionPOAService
    {
        Task<List<ActividadPOA>> getActividadesPOA(string tipo);
        Task<List<OperacionPOA>> getOperacionesPOA();
        Task<CertificacionPOA> createCertificacionPOA(DTOCertificacionPOA request);
    }
}