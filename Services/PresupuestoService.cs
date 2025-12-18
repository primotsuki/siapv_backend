using siapv_backend.Models.DTOResponses;
using siapv_backend.Models.DTORequests;
using siapv_backend.Models;
using siapv_backend.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Operations;
namespace siapv_backend.Services
{
    public class PresupuestoService: IPresupuestoService
    {
        public readonly AppDbContext db;
        public readonly ICorrelativoService corrService;
        public PresupuestoService(AppDbContext _db, ICorrelativoService _corrService)
        {
            db = _db;
            corrService = _corrService;
        }
        public async Task<List<CategoriaProgramatica>> getCategorias()
        {
            return db.categoriasProgramaticas.Where(x=> x.activo==true).ToList();
        }

        public async Task<CertificacionPresupuestaria> createCertificacion(DTOCertificacionPres request)
        {
            var solicitud = await (from s in db.solicitudViajes
                            join tv in db.tiposViaje on s.tipoViajeId equals tv.Id
                            where s.Id == request.solicitudId
                            select new
                            {
                                s, tv
                            }).FirstOrDefaultAsync();
            var estructura = await db.categoriasProgramaticas.FirstOrDefaultAsync(x=>x.Id == request.categoriaId);
            double calculo_dias = getCalculoDias(solicitud.s.fechaInicio,solicitud.s.fechaFin, solicitud.s.horaInicio, solicitud.s.horaFin);
            double importe_solicitado = calculo_dias * solicitud.tv.monto_viatico;
            var saldo = estructura.saldo - importe_solicitado;
            estructura.saldo = saldo;
            CertificacionPresupuestaria cat = new CertificacionPresupuestaria
            {
                solicitudId = request.solicitudId,
                categoriaId = request.categoriaId,
                correlativo = await corrService.getCorrelativoProceso(1), // procesoID 1 se usara para certificaciones presupuestarias
                concepto = request.concepto,
                gestion = DateTime.UtcNow.Year,
                importe_solicitado = importe_solicitado,
                saldo_categoria = saldo,
                createdAt = DateTime.UtcNow,
            };
            db.certificacionPresupuestarias.Add(cat);
            await db.SaveChangesAsync();
            return cat;
        }
        private double getCalculoDias(DateOnly fechaInicio, DateOnly fechaFin, TimeOnly? horaInicio, TimeOnly? horaFin)
        {
            double dias = (fechaFin.DayNumber - fechaInicio.DayNumber) + 1;

            double descuento = 0;

            if (horaInicio >= new TimeOnly(14, 0))
                descuento += 0.5;
            if (horaFin <= new TimeOnly(14, 0))
                descuento += 0.5;

            return dias - descuento;
        }
    }

}
