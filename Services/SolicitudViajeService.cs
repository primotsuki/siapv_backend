using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using siapv_backend.DB;
using siapv_backend.Models;
using siapv_backend.Models.DTORequests;
using siapv_backend.Models.DTOResponses;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Drawing;
using siapv_backend.Helpers;

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
                                join cp in db.certificacionPOAs on sv.Id equals cp.solicitudId
                                into cpoa from cp in cpoa.DefaultIfEmpty()
                                join cpr in db.certificacionPresupuestarias on sv.Id equals cpr.solicitudId
                                into cpres from cpr in cpres.DefaultIfEmpty()
                                join re in db.reprogramaciones on sv.Id equals re.solicitudId
                                into reps from re in reps.DefaultIfEmpty()
                                where sv.empleadoId == empleadoId
                                select new DTOSolicitudViaje
                                {
                                   Id = sv.Id,
                                   fechaInicio = sv.fechaInicio,
                                   fechaFin = sv.fechaFin,
                                   destino = ld.destino,
                                   proyecto = p.descripcion,
                                   estado = e.estado,
                                   estadoId = e.Id,
                                   certPoaId = cp.Id,
                                   certPresId = cpr.Id,
                                   reprogId = re.Id
                                }).ToList();
            return solicitudes;
        }
        public async Task<List<DTOSolicitudesPendientes>> getPendientes(int empleadoId)
        {
            var solicitudes = (from sv in db.solicitudViajes
                                join ld in db.lugarDestinos on sv.lugarDestinoId equals ld.Id
                                join p in db.proyectos on sv.proyectoId equals p.Id
                                join e in db.estadoSolicitudes on sv.estadoId equals e.Id
                                where sv.empleadoId == empleadoId && sv.estadoId < 4
                                select new DTOSolicitudesPendientes
                                {
                                   solicitudId = sv.Id,
                                   fechaInicio = sv.fechaInicio,
                                   fechaFin = sv.fechaFin,
                                   destino = ld.destino,
                                   estado = e.estado,
                                   estadoId = e.Id,
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
                sol.estadoId = 2; // Estado: Solicitado
                sol.horaInicio = solicitud.horaInicio;
                sol.horaFin = solicitud.horaFin;
                sol.lugarOrigenId = solicitud.lugarOrigenId;
                sol.transporteId = solicitud.transporteId;
                sol.tipoViajeId = solicitud.tipoViajeId;
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
                               join re in db.reprogramaciones on s.Id equals re.solicitudId
                                into reps from re in reps.DefaultIfEmpty()
                               select new DTOSolicitudEstado
                               {
                                    solicitudId = s.Id,
                                    estadoId = es.Id,
                                    empleadoId = s.empleadoId,
                                    estado = es.estado,
                                    fechaInicio = s.fechaInicio,
                                    fechaFin = s.fechaFin,
                                    certPoaId = cp.Id,
                                    certPresId = cpr.Id,
                                    reprogId = re.Id
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
            if (request.estadoId==null)
            {
                return solicitudesEmp.OrderByDescending(x=>x.solicitudId).ToList();
            } else
            {
                return solicitudesEmp
                .Where(x=> x.estadoId == request.estadoId)
                .OrderByDescending(x=>x.solicitudId)
                .ToList();
            }

        }
        public async Task<List<DTOSolicitudesPasajes>> getSolicitudesbyParams(DTOsolParams request)
        {
            var query = from u in userDb.Usuarios
                         join p in userDb.Personas
                         on u.PersonaId equals p.Id
                         join e in userDb.EmpleadosContratos on p.Id equals e.personaId
                         select new DTOUserInfo
                         {
                             userId = u.Id,
                             personaId = u.PersonaId,
                             empleadoId = e.Id,
                             cargo = e.DenominacionCargo,
                             nombres = p.Nombres,
                             apellido_paterno = p.apellido_paterno,
                             apellido_materno = p.apellido_materno,
                             SearchVector = p.SearchVector,

                         };
            List<DTOUserInfo> result;
          if (request.searchTerm != null && request.searchTerm.Length > 3)
            {
                result =  query.Where(p => p.SearchVector
                .Matches(EF.Functions.PlainToTsQuery("spanish", request.searchTerm)))
                .Skip((request.page - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToList();
            }
            else
            {
                result = query.
                    Skip((request.page - 1) * request.pageSize)
                    .Take(request.pageSize)
                    .ToList ();
            }
            var solicitudes = (from s in db.solicitudViajes
                                join d in db.lugarDestinos on s.lugarDestinoId equals d.Id
                                join es in db.estadoSolicitudes on s.estadoId equals es.Id
                               join cp in db.certificacionPOAs on s.Id equals cp.solicitudId
                               into cpoa from cp in cpoa.DefaultIfEmpty()
                               join cpr in db.certificacionPresupuestarias on s.Id equals cpr.solicitudId
                               into cpres from cpr in cpres.DefaultIfEmpty()
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
                               }).ToList();
            var solicitudescompiled = (from q in query 
                                        join s in solicitudes
                                        on q.empleadoId equals s.empleadoId
                                        select new DTOSolicitudesPasajes
                                        {
                                            solicitudId = s.solicitudId,
                                            estadoId = s.estadoId,
                                            empleadoId = s.empleadoId,
                                            estado = s.estado,
                                            fechaInicio = s.fechaInicio,
                                            fechaFin = s.fechaFin,
                                            certPoaId = s.certPoaId,
                                            certPresId = s.certPresId,
                                            nombres = q.nombres,
                                            apellido_paterno = q.apellido_paterno,
                                            apellido_materno = q.apellido_materno,
                                        }).ToList();
            return solicitudescompiled;
        }
        public async Task<RevisionFormularios?> crearRevisiondeFormulario(DTORevision request)
        {
            Boolean estado = request.fucav && request.memo && request.presupuesto && request.poa && request.informe;
            RevisionFormularios new_rev = new RevisionFormularios
            {
                fucav = request.fucav,
                memo = request.memo,
                informe = request.memo,
                estadoId = estado ? 6 : 7,
                presupuesto = request.presupuesto,
                createdAt = DateTime.UtcNow,
                solicitudId = request.solicitudId
            };
            db.revisiones.Add(new_rev);
            var sol = await db.solicitudViajes.FirstOrDefaultAsync(x => x.Id == request.solicitudId);
            sol.estadoId = estado ? 6 : 7;
            await db.SaveChangesAsync();
            return new_rev !=null ? new_rev : null;
        }
        public async Task<List<EstadoSolicitud>> getEstados()
        {
            return await db.estadoSolicitudes.ToListAsync();
        }
        public async Task<InformeViaje> createInforme(DTOInformeViaje request)
        {
            var informe = new InformeViaje
            {
                cite_doc = request.cite_doc,
                antecedentes = request.antecedentes,
                conclusion = request.conclusion,
                desarrollo = request.desarrollo,
                solicitudId = request.solicitudId,
                createdAt = DateTime.UtcNow,  
            };
            db.informeViajes.Add(informe);
            await db.SaveChangesAsync();
            return informe;
        }
        public async Task<Byte[]> getInformeViaje(int solicitudId)
        {
            FontManager.RegisterFont(File.OpenRead("Fonts/CenturyGothic.ttf"));
            FontManager.RegisterFont(File.OpenRead("Fonts/CenturyGothic-Bold.ttf"));
            
            var solicitud = await (from s in db.solicitudViajes
                                join d in db.lugarDestinos on s.lugarDestinoId equals d.Id
                                join p in db.proyectos on s.proyectoId equals p.Id
                                join f in db.fuenteFinanciamientos on s.fuenteId equals f.Id
                                where s.Id == solicitudId
                                select new
                                {
                                   s.empleadoId,
                                   s.designadorId,
                                   s.cite_memo,
                                   lugar_destino = d.destino,
                                   proyecto = p.descripcion,
                                   financiamiento = f.descripcion,
                                   s.fechaInicio,
                                   s.fechaFin,
                                   s.horaInicio,
                                   s.horaFin,
                                   s.descripcion_viaje,
                                   s.createdAt
                                }).FirstOrDefaultAsync();
            var empleadoSol = await (from e in userDb.EmpleadosContratos
                                    join p in userDb.Personas on e.personaId equals p.Id
                                    where solicitud.empleadoId == e.Id
                                    select new
                                    {
                                        e, p
                                    }).FirstOrDefaultAsync();
            var empleadoDesignacion = await (from e in userDb.EmpleadosContratos
                                    join p in userDb.Personas on e.personaId equals p.Id
                                    where solicitud.designadorId == e.Id
                                    select new
                                    {
                                        e, p
                                    }).FirstOrDefaultAsync();
            var informe = await db.informeViajes.FirstOrDefaultAsync(x => x.solicitudId == solicitudId);
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter);
                    page.MarginHorizontal(60f);
                    page.MarginVertical(5f);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11));
                    page.DefaultTextStyle(x=>x.FontFamily("Century Gothic"));
                    page.Header().Column(column =>
                    {
                        column.Item().AlignCenter().Width(220).Image("Assets/logo_aisem.png");
                        
                    });
                    page.Content().Element(container =>
                    {
                        container.Column(column =>{
                            column.Item().AlignCenter().Text("INFORME").SemiBold().FontSize(12).FontColor(Colors.Black);
                            column.Item().AlignCenter().PaddingBottom(10).Text(informe.cite_doc).SemiBold().FontSize(12).FontColor(Colors.Black);
                           column.Item().Row( row =>
                           {
                               row.RelativeItem().Element(container =>
                               {
                                  container.Table(table =>
                                  {
                                     table.ColumnsDefinition(columns =>
                                     {
                                        columns.RelativeColumn(3);
                                        columns.RelativeColumn(2);
                                        columns.RelativeColumn(8);
                                        columns.RelativeColumn(2);
                                     });
                                    table.Cell().RowSpan(9).BorderBottom(1).Text("");
                                    table.Cell().RowSpan(2).Text("A:").Bold();
                                    table.Cell().Text($"{empleadoDesignacion.p.Nombres} {empleadoDesignacion.p.apellido_paterno} {empleadoDesignacion.p.apellido_materno}");
                                    table.Cell().RowSpan(8).BorderBottom(1).Text("");
                                    table.Cell().PaddingBottom(5).Text($"{empleadoDesignacion.e.DenominacionCargo}".ToUpper()).SemiBold();
                                    table.Cell().RowSpan(2).Text("DE:").Bold();
                                    table.Cell().Text($"{empleadoSol.p.Nombres} {empleadoSol.p.apellido_paterno} {empleadoSol.p.apellido_materno}");
                                    table.Cell().PaddingBottom(5).Text($"{empleadoSol.e.DenominacionCargo}".ToUpper()).SemiBold();
                                    table.Cell().Text("REF.:").SemiBold();
                                    table.Cell().PaddingBottom(5).Text("INFORME DE DESCARGO DE VIAJE EN COMISIÓN").SemiBold();                                  
                                    table.Cell().Text("FECHA:").SemiBold();
                                    table.Cell().Text($"{informe.createdAt.ToLongDateString()}");
                                    table.Cell().ColumnSpan(2).BorderBottom(1).Text("");
                                  });
                               });
                           });
                           column.Item().Row(row =>
                           {
                               row.RelativeItem().Element( container =>
                               {
                                   container.Table( table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.RelativeColumn(4);
                                           columns.RelativeColumn(4);
                                           columns.RelativeColumn(4);
                                           columns.RelativeColumn(4);

                                           table.Cell().PaddingTop(10).Text("DESTINO:");
                                           table.Cell().ColumnSpan(3).PaddingTop(10).Border(1).Height(30).Text($"{solicitud.lugar_destino}").AlignCenter();
                                            table.Cell().Padding(5).Text("PROYECTO");
                                            table.Cell().ColumnSpan(3).Padding(5).Border(1).Height(30).Text($"{solicitud.proyecto}");
                                            table.Cell().Padding(5).Text("Fecha de Salida");
                                            table.Cell().Padding(5).Border(1).Height(30).Text($"{solicitud.fechaInicio.ToShortDateString()}").AlignCenter();
                                            table.Cell().Padding(5).Text("Fecha de Retorno");
                                            table.Cell().Padding(5).Border(1).Height(30).Text($"{solicitud.fechaInicio.ToShortDateString()}").AlignCenter();
                                            table.Cell().Padding(5).Text("Hora de Salida");
                                            table.Cell().Padding(5).Border(1).Height(30).Text($"{solicitud.horaInicio.ToString()}").AlignCenter();
                                            table.Cell().Padding(5).Text("Hora de Retorno");
                                            table.Cell().Padding(5).Border(1).Height(30).Text($"{solicitud.horaFin.ToString()}").AlignCenter();
                                            table.Cell().ColumnSpan(4).BorderBottom(1).Text("");
                                       });
                                   });
                               });
                           });
                            var parser = new HtmlParser();
                            column.Item().Padding(10).Text("1. ANTECEDENTES.").Bold();
                            parser.Render(informe.antecedentes, column);
                            column.Item().Padding(10).Text("2. DESARROLLO.").Bold();
                            parser.Render(informe.antecedentes, column);
                            column.Item().Padding(10).Text("3. CONCLUSIONES Y RECOMENDACIONES.").Bold();
                            parser.Render(informe.antecedentes, column);
                        });
                    });
                });
            });
            return document.GeneratePdf();
        }
        public async Task<InformeViaje> getInformeEdit(int solicitudId)
        {
            return new InformeViaje();
        }
        public async Task<InformeViaje> editInforme(DTOInformeViaje request)
        {
            return new InformeViaje();
        }
    }
}