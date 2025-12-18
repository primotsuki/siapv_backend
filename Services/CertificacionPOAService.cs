using siapv_backend.Models.DTORequests;
using siapv_backend.Models;
using siapv_backend.DB;
using Microsoft.EntityFrameworkCore;
namespace siapv_backend.Services
{
    public class CertificacionPOAService: ICertificacionPOAService
    {
        public readonly AppDbContext db;
        public readonly ICorrelativoService corrService;
        public CertificacionPOAService(AppDbContext _db, ICorrelativoService _cos)
        {
            db = _db;
            corrService = _cos;
        }
        public async Task<List<ActividadPOA>> getActividadesPOA(string tipo)
        {
            return db.actividadPOAs.Where(x =>x.tipo == tipo).ToList();
        }
        public async Task<List<OperacionPOA>> getOperacionesPOA()
        {
            return db.operacionPOAs.Where(x=> x.activo).ToList();
        }
        public async Task<CertificacionPOA> createCertificacionPOA(DTOCertificacionPOA request)
        {
            CertificacionPOA cert = new CertificacionPOA
            {
                actividadAcpId = request.actividadAcpId,
                actividadAmpId = request.actividadAmpId,
                solicitudId = request.solicitudId,
                correlativo = await corrService.getCorrelativoProceso(2), // proceso 2 para certificaciones POA
                createdAt = DateTime.UtcNow,
                gestion = DateTime.UtcNow.Year
            };
            db.certificacionPOAs.Add(cert);
            await db.SaveChangesAsync();
            return cert;
        }
    }    
}