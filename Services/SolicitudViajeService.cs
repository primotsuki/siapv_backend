using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using siapv_backend.DB;
using siapv_backend.Models;
using siapv_backend.Models.DTORequests;
using siapv_backend.Models.DTOResponses;

namespace siapv_backend.Services
{
    public class SolicitudViajeService: ISolicitudViajeService
    {
        public readonly AppDbContext db;
        public readonly DBUsuariosContext userDb;

        public SolicitudViajeService(AppDbContext _db, DBUsuariosContext _usrDb)
        {
            db = _db;
            userDb = _usrDb;
        }
        public async Task<List<LugarDestino>> GetDestinos()
        {
            return db.lugarDestinos.ToList();
        }
        public async Task<List<TipoViaje>> getTiposViaje()
        {
            return db.tiposViaje.ToList();
        }
        public async Task<SolicitudViaje?> generarMemorandum(DTOMemorandum memo, int designadorId)
        {
            var viaje = new SolicitudViaje
            {
                fechaInicio = memo.fechaInicio,
                fechaFin = memo.fechaFin,
                descripcion_viaje = memo.descripcion_viaje,
                cite_memo = memo.cite_memo,
                proyectoId = memo.proyectoId,
                lugarDestinoId = memo.lugarDestinoId,
                fuenteId = memo.fuenteId, 
                empleadoId = memo.empleadoId,
                designadorId = designadorId,
                createdAt = DateTime.UtcNow,
                estadoId = 1 // Estado: Designado
            };
            db.solicitudViajes.Add(viaje);
            var result = await db.SaveChangesAsync();
            return result >= 0 ? viaje : null;
        }
        public async Task<List<DTOSolicitudViaje>> getSolicitudes(int empleadoId)
        {
            var solicitudes = (from sv in db.solicitudViajes
                                join ld in db.lugarDestinos on sv.lugarDestinoId equals ld.Id
                                join p in db.proyectos on sv.proyectoId equals p.Id
                                join e in db.estadoSolicitudes on sv.estadoId equals e.Id
                               where sv.empleadoId == empleadoId
                               select new DTOSolicitudViaje
                               {
                                   Id = sv.Id,
                                   fechaInicio = sv.fechaInicio,
                                   fechaFin = sv.fechaFin,
                                   destino = ld.destino,
                                   proyecto = p.descripcion,
                                   estado = e.estado,
                                   estadoId = e.Id
                               }).ToList();
            return solicitudes;
        }
        public async Task<List<DTOEmpleadoSolicitud>> getSolicitudesByDependencia(int dependenciaId)
        {

            var solicitudes = await (from s in db.solicitudViajes
                               join es in db.estadoSolicitudes on s.estadoId equals es.Id
                               select new {s, es}).ToListAsync();
            
            var empleados = await ( from em in userDb.EmpleadosContratos
                               join p in userDb.Personas on em.personaId equals p.Id
                               join dep in userDb.Dependencias on em.DependenciaId equals dep.Id
                               where dep.dependenciaId == dependenciaId || dep.Id == dependenciaId
                               select new {em, p}).ToListAsync();

            var solicitudesEmp = from sol in solicitudes
                                 join emp in empleados on sol.s.empleadoId equals emp.em.Id
                                 select new DTOEmpleadoSolicitud
                                 {
                                     Id = sol.s.Id,
                                     empleadoId = emp.em.Id,
                                     nombres = emp.p.Nombres,
                                     apellido_paterno = emp.p.apellido_paterno,
                                     apellido_materno = emp.p.apellido_materno,
                                     estado = sol.es.estado,
                                     fechaInicio = sol.s.fechaInicio,
                                     fechaFin = sol.s.fechaFin,
                                 };
            return solicitudesEmp.ToList();
        }
        public async Task<SolicitudViaje?> completarSolicitud(DTOFucav solicitud)
        {
            var sol = await db.solicitudViajes.FindAsync(solicitud.Id);
            if (solicitud != null)
            {
                sol.estadoId = 2; // Estado: Completado
                sol.horaInicio = solicitud.horaInicio;
                sol.horaFin = solicitud.horaFin;
                sol.lugarOrigenId = solicitud.lugarOrigenId;
                sol.transporteId = solicitud.transporteId;
                sol.updatedAt = DateTime.UtcNow;
                db.solicitudViajes.Update(sol);
                var result = await db.SaveChangesAsync();
                return result >= 0 ? sol : null;
            }
            return null;
        }
        public async Task<SolicitudViaje?> getSolicitudById(int solicitudId)
        {
            var solicitud = await db.solicitudViajes.FindAsync(solicitudId);
            return solicitud;
        }
        public async Task<List<MediosTransporte>> getMediosTransporte()
        {
            return await db.mediosTransportes.ToListAsync();
        }

        public async Task<List<DTOSolicitudEstado>> getSolicitudByEstados(DTOsolicitudReq request)
        {
            var solicitudes = await (from s in db.solicitudViajes
                               join es in db.estadoSolicitudes on s.estadoId equals es.Id
                               join cp in db.certificacionPOAs on s.Id equals cp.solicitudId
                               into cpoa from cp in cpoa.DefaultIfEmpty()
                               join cpr in db.certificacionPresupuestarias on s.Id equals cpr.solicitudId
                               into cpres from cpr in cpres.DefaultIfEmpty()
                               where es.Id == request.estadoId
                               select new DTOSolicitudEstado
                               {
                                    solicitudId = s.Id,
                                    estadoId = es.Id,
                                    empleadoId = s.empleadoId,
                                    estado = es.estado,
                                    fechaInicio = s.fechaInicio,
                                    fechaFin = s.fechaFin,
                                    certPoaId = cp.Id,
                                    certPresId = cpr.Id
                               }).ToListAsync();
            
            var empleados = await ( from em in userDb.EmpleadosContratos
                               join p in userDb.Personas on em.personaId equals p.Id
                               join dep in userDb.Dependencias on em.DependenciaId equals dep.Id
                               select new {em, p}).ToListAsync();

            var solicitudesEmp = (from sol in solicitudes
                                 join emp in empleados on sol.empleadoId equals emp.em.Id
                                 select new DTOSolicitudEstado
                                 {
                                     solicitudId = sol.solicitudId,
                                     empleadoId = emp.em.Id,
                                     nombres = emp.p.Nombres,
                                     apellido_paterno = emp.p.apellido_paterno,
                                     apellido_materno = emp.p.apellido_materno,
                                     estadoId = sol.estadoId,
                                     estado = sol.estado,
                                     fechaInicio = sol.fechaInicio,
                                     fechaFin = sol.fechaFin,
                                     certPoaId =sol.certPoaId,
                                     certPresId = sol.certPresId,
                                 }).ToList();
            return solicitudesEmp;
        }
    }
}