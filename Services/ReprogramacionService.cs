using siapv_backend.DB;
using siapv_backend.Models;
using siapv_backend.Models.DTORequests;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
namespace siapv_backend.Services
{
    public class ReprogramacionService: IReprogramacionService
    {
        private readonly AppDbContext _db;
        private readonly DBUsuariosContext _usrDb;


        public ReprogramacionService( AppDbContext db, DBUsuariosContext usrDb)
        {
            _db = db;
            _usrDb = usrDb;
        }

        public async Task<Reprogramacion> createReprogramacion(DTOReprogramacion request)
        {
            var new_reprog = new Reprogramacion
            {
                destinoId = request.destinoId,
                nro_boleto = request.nro_boleto,
                origenId = request.origenId,
                linea = request.linea,
                fecha_emsision = request.fecha_emision,
                solicitudId = request.solicitudId,
                importe = request.importe,
                justificacion = request.justificacion,
                createAt = DateTime.UtcNow
            };
            _db.reprogramaciones.Add(new_reprog);
            var solicitud = await _db.solicitudViajes.FirstOrDefaultAsync(x=>x.Id == request.solicitudId);
            solicitud.estadoId = 4; // reprogramado
             await _db.SaveChangesAsync();
            return new_reprog;
        }
        public async Task<Byte[]> getFormularioReprogramacion(int solicitudId)
        {
            var solicitud = await _db.solicitudViajes.FirstOrDefaultAsync(x => x.Id == solicitudId);
            var empleadoSol = await (from e in _usrDb.EmpleadosContratos
                                    join p in _usrDb.Personas on e.personaId equals p.Id
                                    join d in _usrDb.Dependencias on e.DependenciaId equals d.Id
                                    where solicitud.empleadoId == e.Id
                                    select new
                                    {
                                        e, p, d
                                    }).FirstOrDefaultAsync();
            var reprog = await (
                from r in _db.reprogramaciones
                join o in _db.lugarDestinos on r.origenId equals o.Id
                join d in _db.lugarDestinos on r.destinoId equals d.Id
                where r.solicitudId == solicitudId
                select new {
                r,o,d
            }
            ).FirstOrDefaultAsync();
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter);
                    page.MarginHorizontal(40f);
                    page.MarginVertical(40f);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Column(column =>
                    {
                       column.Item().AlignCenter().Width(220).Image("Assets/logo_aisem.png");
                        column.Item().AlignCenter().Text("500 - PASAJES AÉREOS PENDIENTES DE USO").SemiBold().FontSize(12).FontColor(Colors.Black);
                    });
                    page.Content().Element(container =>
                    {
                        container.Column(column =>
                        {
                            column.Item().PaddingVertical(5).Row( row =>
                            {
                                row.RelativeItem().Element(container =>
                                {
                                   container.Table(table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(3);
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(2);
                                       });     
                                        table.Cell().Border(1).Text("Nombre Completo").SemiBold();
                                        table.Cell().Border(1).Text($"{empleadoSol.p.Nombres} {empleadoSol.p.apellido_paterno} {empleadoSol.p.apellido_materno}");
                                        table.Cell().Border(1).Text("Carnet:").SemiBold();
                                        table.Cell().Border(1).Text($"{empleadoSol.p.carnet}");
                                   });
                                });
                            });

                            column.Item()
                            .PaddingVertical(5)
                            .Text("A través del presente formulario solicito se registre el(los) siguiente(s) Pasaje(s) Pendiente(s) de Uso por concepto de pasaje(s) aéreo(s) emitido(s) a mi nombre bajo el siguiente detalle:")
                            .Justify();
                            column.Item()
                            .PaddingVertical(5)
                            .Text("Pasaje Aereo")
                            .SemiBold();

                            column.Item().PaddingVertical(5).Row( row =>
                            {
                                row.RelativeItem().Element(container =>
                                {
                                   container.Table(table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                            columns.RelativeColumn(1);
                                            columns.RelativeColumn(3);
                                            columns.RelativeColumn(3);
                                            columns.RelativeColumn(3);
                                            columns.RelativeColumn(3);
                                            columns.RelativeColumn(3);
                                       });
                                        table.Cell().Border(1).Text("N°").AlignCenter().SemiBold();
                                        table.Cell().Border(1).Text("LINEA AÉREA").AlignCenter().SemiBold();
                                        table.Cell().Border(1).Text("ORIGEN").AlignCenter().SemiBold();
                                        table.Cell().Border(1).Text("DESTINO").AlignCenter().SemiBold();
                                        table.Cell().Border(1).Text("NUMERO DE BOLETO").AlignCenter().SemiBold();
                                        table.Cell().Border(1).Text("IMPORTE").AlignCenter().SemiBold();
                                        table.Cell().Border(1).Text("1").AlignCenter();
                                        table.Cell().Border(1).Text($"{reprog.r.linea}").AlignCenter();
                                        table.Cell().Border(1).Text($"{reprog.o.destino}").AlignCenter();
                                        table.Cell().Border(1).Text($"{reprog.d.destino}").AlignCenter();
                                        table.Cell().Border(1).Text($"{reprog.r.nro_boleto}").AlignCenter();
                                        table.Cell().Border(1).Text($"{reprog.r.importe.ToString("0.00")}").AlignCenter();
                                        table.Cell().ColumnSpan(5).Border(1).Text("TOTAL").SemiBold().AlignEnd();
                                        table.Cell().Border(1).Text($"{reprog.r.importe.ToString("0.00")}").AlignCenter();
                                   });
                                });
                            });
                            column.Item()
                            .PaddingVertical(5)
                            .Text("Justificación")
                            .SemiBold();
                           column.Item()
                            .PaddingVertical(5)
                            .Border(1)
                            .Text($"{reprog.r.justificacion}")
                            .Justify();
                            column.Item()
                            .PaddingVertical(5)
                            .Text("En el plazo de vigencia del pasaje aereo me comprometo a reprogramar el/los pasaje(s) detallado(s) en el cuadro anterior en cumplimiento al Artículo.- 31, paragrafo III y IV del Reglamento Interno de Pasajes y Viaticos Vigente. Asimismo declaro la veracidad de los documentos y la información precedente y autorizo su verificación, caso contrario me someto a sanciones establecidas en el Marco por la Función Pública")
                            .Justify();
                        });
                    });
                });
            });
            return document.GeneratePdf();
        }
    }
}