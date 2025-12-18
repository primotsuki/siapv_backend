using siapv_backend.Models;
namespace siapv_backend.Services
{
    public interface IReportService
    {
        Task<Byte[]> getMemorandumDoc(int solicitudId);
        Task<Byte[]> getFucavDoc(int solicitudId);
        Task<Byte[]> getCertificacionPOA(int solicitudId);
        Task<Byte[]> getCertificacionPresupuestaria(int solicitudId);
    }
}