using siapv_backend.DB;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using QuestPDF.Drawing;
using System.ComponentModel;
using siapv_backend.Models;

namespace siapv_backend.Services
{
    public class ReportService: IReportService {
        public readonly AppDbContext db;
        public readonly DBUsuariosContext userDb;
        public ReportService(AppDbContext _db, DBUsuariosContext _userDb)
        {
            db =_db;
            userDb = _userDb;
        }
        public async Task<Byte[]> getMemorandumDoc(int solicitudId)
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
                        column.Item().AlignCenter().Text("MEMORANDUM").SemiBold().FontSize(15).FontColor(Colors.Black);
                        column.Item().AlignCenter().PaddingBottom(10).Text(solicitud.cite_memo).SemiBold().FontSize(15).FontColor(Colors.Black);
                    });
                    page.Content().Element(container =>
                    {
                        container.Column(column =>
                        {
                            column.Item().Row( row =>
                            {
                                row.RelativeItem().Element(container =>
                                {
                                   container.Table(table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.RelativeColumn(4);
                                           columns.RelativeColumn(0.8f);
                                           columns.RelativeColumn(4);
                                       });

                                       table.Cell().ColumnSpan(1).RowSpan(4).Text("");
                                       table.Cell().ColumnSpan(1).RowSpan(2).BorderLeft(1).Element(padCell("De:"));
                                       table.Cell().Padding(3).Text($"{empleadoDesignacion.p.Nombres} {empleadoDesignacion.p.apellido_paterno} {empleadoDesignacion.p.apellido_materno}");
                                       table.Cell().Padding(3).Text($"{empleadoDesignacion.e.DenominacionCargo}").SemiBold();
                                       table.Cell().ColumnSpan(1).RowSpan(2).BorderLeft(1).Element(padCell("A:"));
                                       table.Cell().Padding(3).Text($"{empleadoSol.p.Nombres} {empleadoSol.p.apellido_paterno} {empleadoSol.p.apellido_materno}");
                                       table.Cell().Padding(3).Text($"{empleadoSol.e.DenominacionCargo}").SemiBold();
                                       table.Cell().BorderBottom(1).Element(padCell(solicitud.createdAt.ToLongDateString()));
                                       table.Cell().BorderLeft(1).BorderBottom(1).Element(padCell("Ref.:"));
                                       table.Cell().BorderBottom(1).Element(padCell("VIAJE EN COMISION OFICIAL"));
                                   });
                                });
                            });
                            column.Item()
                                .PaddingVertical(5)
                                .Text("De mi Consideracion:");

                            column.Item()
                                .PaddingVertical(5)
                                .Text("Por Medio de la presente, instruyo a usted el siguiente Viaje en Comision Oficial:");
                            column.Item().Row( row =>
                            {
                               row.RelativeItem().Element( container =>
                               {
                                   container.Table( table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(0.5f);
                                           columns.RelativeColumn(3);
                                           columns.RelativeColumn(0.5f);
                                           columns.RelativeColumn(3);
                                       });     
                                        table.Cell().Border(1).AlignRight().Element(padCell("LUGAR: "));
                                        table.Cell().ColumnSpan(4).Border(1).Element(padCell(solicitud.lugar_destino));
                                        table.Cell().Border(1).AlignRight().Element(padCell("FECHA"));
                                        table.Cell().Border(1).Element(padCell("Del"));
                                        table.Cell().Border(1).Element(padCell(solicitud.fechaInicio.ToString()));
                                        table.Cell().Border(1).Element(padCell("Al: "));
                                        table.Cell().Border(1).Element(padCell(solicitud.fechaFin.ToString()));
                                        table.Cell().Border(1).AlignRight().Element(padCell("PROYECTO: "));
                                        table.Cell().ColumnSpan(4).Border(1).Element(padCell(solicitud.proyecto));
                                        table.Cell().Border(1).AlignRight().Element(padCell("OBJETO: "));
                                        table.Cell().ColumnSpan(4).Border(1).Element(padCell(solicitud.descripcion_viaje));
                                        table.Cell().Border(1).AlignRight().Element(padCell("FINANCIAMIENTO: "));
                                        table.Cell().ColumnSpan(4).Border(1).Element(padCell(solicitud.financiamiento));
                                   });
                               });
                            });
                            column.Item()
                                    .PaddingVertical(5)
                                    .Text("El pago de pasajes y viáticos por los días señalados, serán cancelados por la Dirección de Administración y Finanzas de la Agencia de Infraestructura en Salud y Equipamiento Médico (AISEM).")
            
                                    .Justify();
                              
                            column.Item()
                                    .PaddingVertical(5)
                                    .Text("Concluido el Viaje en Comisión Oficial, debe presentar a la Dirección de Administración y Finanzas el Informe de descargo de viaje en comisión y demas docuentación requerida en el plazo máximo de ocho (8) días habiles en estricto cumplimiento al Artículo N° 7 del Decreto Supremo N° 1788 y Artículo N° 25 del Reglamento Interno de Pasajes y viáticos.")
                                    .ParagraphSpacing(30)
                                    .Justify();
                            column.Item()
                                .PaddingVertical(5)
                                .Text("Con este particular, me despido atentamente.").Justify();
                        });
                    });
                });
            });
            return document.GeneratePdf();
        }
        private static Action<QuestPDF.Infrastructure.IContainer> padCell(string text)
        {
            return container =>
            {
                container
                    .Padding(5)
                    .Text(text);
            };
        }

        public async Task<Byte[]> getFucavDoc(int solicitudId)
        {
            var solicitud = await (from s in db.solicitudViajes
                                join d in db.lugarDestinos on s.lugarDestinoId equals d.Id
                                join p in db.proyectos on s.proyectoId equals p.Id
                                join f in db.fuenteFinanciamientos on s.fuenteId equals f.Id
                                join d1 in db.lugarDestinos on s.lugarOrigenId equals d1.Id
                                join t in db.mediosTransportes on s.transporteId equals t.Id
                                join tv in db.tiposViaje on s.tipoViajeId equals tv.Id
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
                                   s.createdAt,
                                   s.updatedAt,
                                   t.transporte,
                                   lugar_origen = d1.destino,
                                   tipo_viaje = tv.tipo,
                                   tv.monto_viatico
                                }).FirstOrDefaultAsync();
            var empleadoSol = await (from e in userDb.EmpleadosContratos
                                    join p in userDb.Personas on e.personaId equals p.Id
                                    join d in userDb.Dependencias on e.DependenciaId equals d.Id
                                    where solicitud.empleadoId == e.Id
                                    select new
                                    {
                                        e, p, d
                                    }).FirstOrDefaultAsync();
            double cantidad_dias = getCalculoDias(solicitud.fechaInicio, solicitud.fechaFin, solicitud.horaInicio, solicitud.horaFin);
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter);
                    page.MarginHorizontal(60f);
                    page.MarginVertical(5f);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));
                    
                    page.Header().Column(column =>
                    {
                        column.Item().AlignCenter().Width(110).Image("Assets/logo_aisem.png");
                        column.Item().AlignCenter().Text("200 - FORMULARIO UNICO DE COMISION Y AUTORIZACION DE VIAJE").SemiBold().FontSize(12).FontColor(Colors.Black);
                    });
                    page.Content().Element(container =>
                    {
                        container.Column(column =>
                        {
                            column.Item().Row( row =>
                            {
                                row.RelativeItem().Element(container =>
                                {
                                   container.Table(table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.RelativeColumn(1.5f);
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(3);
                                           columns.RelativeColumn(1.3f);
                                           columns.RelativeColumn(2);
                                       });

                                       table.Cell().ColumnSpan(1).RowSpan(2).Image("Assets/logo_normal.png");
                                       table.Cell().Border(1).Text("TIPO DE VIAJE: ");
                                       table.Cell().Border(1).Text(solicitud.tipo_viaje);
                                       table.Cell().Border(1).Text("CATEGORIA");
                                       table.Cell().Border(1).Text("TERCERA");
                                       table.Cell().Border(1).Text("FINANCIAMIENTO");
                                       table.Cell().Border(1).Text(solicitud.financiamiento);
                                       table.Cell().Border(1).Text("FECHA");
                                       table.Cell().Border(1).Text(solicitud.updatedAt.ToString()).FontSize(9);
                                   });
                                });
                            });
                            column.Item().Row( row =>
                            {
                                row.RelativeItem().Element(container =>
                                {
                                   container.Table(table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(4);
                                           columns.RelativeColumn(1);
                                           columns.RelativeColumn(1);
                                       });
                                       table.Cell().ColumnSpan(3).Border(1).Text("1. Datos del solicitante y Declaratoria en Comisión").SemiBold();
                                       table.Cell().Text("");
                                       table.Cell().Border(1).Text("Nombres y Apellidos");
                                       table.Cell().Border(1).Text($"{empleadoSol.p.Nombres} {empleadoSol.p.apellido_paterno} {empleadoSol.p.apellido_materno}");
                                       table.Cell().Border(1).Text("C.I.");
                                       table.Cell().Border(1).Text(empleadoSol.p.carnet);
                                       table.Cell().Border(1).Text("Dependencia:");
                                       table.Cell().ColumnSpan(3).Border(1).Text(empleadoSol.d.descripcion);
                                       table.Cell().Border(1).Text("Cargo que desempeña");
                                       table.Cell().ColumnSpan(3).Border(1).Text(empleadoSol.e.DenominacionCargo);
                                       table.Cell().Border(1).Text("Nro de Item o Contrato");
                                       table.Cell().ColumnSpan(3).Border(1).Text(empleadoSol.e.nro_contrato);
                                       table.Cell().Border(1).Text("Descripción y/o Objeto de la comisión de Viaje");
                                       table.Cell().ColumnSpan(3).Border(1).Text(solicitud.descripcion_viaje);
                                       table.Cell().Border(1).Text("Proyecto 1:");
                                       table.Cell().ColumnSpan(3).Border(1).Text(solicitud.proyecto);
                                       table.Cell().Border(1).Text("Proyecto 2");
                                       table.Cell().ColumnSpan(3).Border(1).Text("");
                                       table.Cell().Border(1).Text("Proyecto 3");
                                       table.Cell().ColumnSpan(3).Border(1).Text("");
                                       table.Cell().Border(1).Text("Memorandum o Nota Interna N°");
                                       table.Cell().Border(1).Text(solicitud.cite_memo);
                                       table.Cell().Border(1).Text("Fecha Emisión");
                                       table.Cell().Border(1).Text(solicitud.createdAt.ToString());
                                   }); 
                                }); 
                            });
                            column.Item().Row( row =>
                            {
                                row.RelativeItem().Element(container =>
                                {
                                   container.Table(table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.RelativeColumn(1);
                                           columns.RelativeColumn(1);
                                           columns.RelativeColumn(1);
                                           columns.RelativeColumn(1);
                                           columns.RelativeColumn(1);
                                       });
                                        table.Cell().ColumnSpan(4).Border(1).Text("2. Datos del Viaje").SemiBold();
                                        table.Cell().Border(1).Text("Porcentaje").AlignCenter();
                                        table.Cell().ColumnSpan(2).Border(1).Text("INICIO").SemiBold().AlignCenter();   
                                        table.Cell().ColumnSpan(2).Border(1).Text("CONCLUSIÓN").SemiBold().AlignCenter();
                                        table.Cell().Border(1).Text("100%").AlignCenter();                                       
                                        table.Cell().Border(1).Text("FECHA").SemiBold().AlignCenter();
                                        table.Cell().Border(1).Text("HORA").SemiBold().AlignCenter();
                                        table.Cell().Border(1).Text("FECHA").SemiBold().AlignCenter();
                                        table.Cell().Border(1).Text("HORA").SemiBold().AlignCenter();
                                        table.Cell().Border(1).Text("Total Dias").SemiBold().AlignCenter();
                                        table.Cell().Border(1).Text(solicitud.fechaInicio.ToString()).AlignCenter();
                                        table.Cell().Border(1).Text(solicitud.horaInicio.ToString()).AlignCenter();
                                        table.Cell().Border(1).Text(solicitud.fechaFin.ToString()).AlignCenter();
                                        table.Cell().Border(1).Text(solicitud.horaFin.ToString()).AlignCenter();
                                        table.Cell().Border(1).Text(cantidad_dias.ToString()).AlignCenter();
                                        table.Cell().ColumnSpan(2).Border(1).Text("Ruta de la Comisión");
                                        table.Cell().Border(1).Text("Medio de Transporte: ");
                                        table.Cell().ColumnSpan(2).Border(1).Text(solicitud.transporte).AlignCenter();
                                        table.Cell().Border(1).Text("Lugar de Origen");
                                        table.Cell().Border(1).Text("Lugar de Destino");
                                        table.Cell().ColumnSpan(3).RowSpan(4).Border(1).Text("SOLICITADO POR").AlignCenter();
                                        table.Cell().Border(1).Text(solicitud.lugar_origen).AlignCenter();
                                        table.Cell().Border(1).Text(solicitud.lugar_destino);
                                        table.Cell().ColumnSpan(2).Border(1).Text("3. Solicitud y Liquidación de Viaticos");
                                        table.Cell().ColumnSpan(2).Border(1).Text("A traves del presente Formulario de Comisión y Autorización de viaticos (FUCAV), tengo a bien solicitar la asignación de viaticos para el cumplimiento de mis actividades programadas de acuerdo a toda la información proporcionado.")
                                        .FontSize(9).Justify();
                                   }); 
                                }); 
                            });
                            column.Item().Row( row =>
                            {
                                row.RelativeItem().Element(container =>
                                {
                                   container.Table(table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.RelativeColumn(4);
                                           columns.RelativeColumn(1);
                                           columns.RelativeColumn(1);
                                           columns.RelativeColumn(1);
                                       });
                                        table.Cell().ColumnSpan(2).Border(1).Text("VIATICO DIARIO POR CATEGORIA DE CARGO").SemiBold();
                                        table.Cell().ColumnSpan(2).RowSpan(3).Border(1).Text("Directores, Asesores, Jefes de Unidad, Profesionales, Tecnicos y otros no compredidos en la anterios categoria.").AlignCenter();
                                        table.Cell().Border(1).Text("ASIGNADO POR EL D.S. 1788: Tipo de cambio");
                                        table.Cell().Border(1).Text("6,96");
                                        table.Cell().RowSpan(5).Border(1).Text("VIATICOS 100% \n Pago de Viacticos al 100% \n Segun Decreto Supremo N° 1788, Art. 4 (Escala de Viaticos) y Reglamento Interno de Pasajes y Viaticos");
                                        table.Cell().Border(1).Text("100%").AlignCenter();
                                        table.Cell().Border(1).Text("Detalle de la liquidación").AlignCenter();
                                        table.Cell().Border(1).Text("USD").AlignCenter();
                                        table.Cell().Border(1).Text("Bs.").AlignCenter();
                                        table.Cell().Border(1).Text("Viatico por dia");
                                        table.Cell().Border(1).Text("0.00").AlignCenter();
                                        table.Cell().Border(1).Text(solicitud.monto_viatico.ToString("0.00")).AlignCenter();
                                        table.Cell().Border(1).Text("Dias de Comisión");
                                        table.Cell().Border(1).Text("0,0").AlignCenter();
                                        table.Cell().Border(1).Text(cantidad_dias.ToString()).AlignCenter();
                                        table.Cell().Border(1).Text("Total Viaticos");
                                        table.Cell().Border(1).Text("0,00").AlignCenter();
                                        table.Cell().Border(1).Text((cantidad_dias * solicitud.monto_viatico).ToString("0.00")).AlignCenter();
                                   }); 
                                }); 
                            });
                            column.Item().Row( row =>
                            {
                                row.RelativeItem().Element(container =>
                                {
                                   container.Table(table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.RelativeColumn(5);
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(2);
                                       });
                                        table.Cell().ColumnSpan(3).Border(1).Text("Calculo a ejecutar cuando los consultores Individuales de Linea no presenten descargo RC-IVA, lo presenten mal o funcionarios desvinculados de la Institución").SemiBold();
                                        table.Cell().Border(1).Text("");
                                        table.Cell().Border(1).Text("USD.-").AlignCenter();
                                        table.Cell().Border(1).Text("Bs.").AlignCenter();
                                        table.Cell().Border(1).Text("CALCULO RC-IVA 13%");
                                        table.Cell().Border(1).Text("0.00").AlignCenter();
                                        table.Cell().Border(1).Text((cantidad_dias * solicitud.monto_viatico * 0.13).ToString("0.00")).AlignCenter();
                                        table.Cell().Border(1).Text("LIQUIDO PAGABLE");
                                        table.Cell().Border(1).Text("0,00").AlignCenter();
                                        table.Cell().Border(1).Text((cantidad_dias * solicitud.monto_viatico * 0.87).ToString("0.00")).AlignCenter();
                                   }); 
                                }); 
                            });
                            column.Item().Row( row =>
                            {
                                row.RelativeItem().Element(container =>
                                {
                                   container.Table(table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.RelativeColumn(1);
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(3);
                                       });
                                        table.Cell().ColumnSpan(3).Text("4. Aprobación y/o Autorización (Vo. Bo. Firma, Sello y Recepción)").SemiBold();
                                        table.Cell().Border(1).Height(90).Text("APROBADO POR").FontSize(9).AlignCenter();
                                        table.Cell().Border(1).Text("AUTORIZADO POR").FontSize(9).AlignCenter();
                                        table.Cell().Border(1).Text("Recepción de Pasajes y Viaticos").FontSize(9).AlignCenter();
                                        table.Cell().ColumnSpan(3).Text("5. Certificación POA y presupuesto").SemiBold();
                                        table.Cell().ColumnSpan(2).Border(1).Height(90).Text("Certificación POA").FontSize(9).AlignCenter();
                                        table.Cell().Border(1).Text("Certificación Presupuestaria").FontSize(9).AlignCenter();
                                   }); 
                                }); 
                            });
                        });
                    });
                });
            });
            return document.GeneratePdf();
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
  
        public async Task<Byte[]> getCertificacionPOA(int solicitudId)
        {
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
                                   s.descripcion_viaje,
                                   s.createdAt
                                }).FirstOrDefaultAsync();
            var empleadoSol = await (from e in userDb.EmpleadosContratos
                                    join p in userDb.Personas on e.personaId equals p.Id
                                    join d in userDb.Dependencias on e.DependenciaId equals d.Id
                                    where solicitud.empleadoId == e.Id
                                    select new
                                    {
                                        e, p, d
                                    }).FirstOrDefaultAsync();
            var empleadoDesignacion = await (from e in userDb.EmpleadosContratos
                                    join p in userDb.Personas on e.personaId equals p.Id
                                    where solicitud.designadorId == e.Id
                                    select new
                                    {
                                        e, p
                                    }).FirstOrDefaultAsync();
            
            var certificacion = await (from ce in db.certificacionPOAs
                                        join acp in db.actividadPOAs on ce.actividadAcpId equals acp.Id
                                        join amp in db.actividadPOAs on ce.actividadAmpId equals amp.Id
                                        join op in db.operacionPOAs on ce.operacionId equals op.Id
                                        where ce.solicitudId == solicitudId
                                        select new
                                        {
                                            ce, acp, amp, op
                                        }).FirstOrDefaultAsync();

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
                       column.Item().Row( row =>
                            {
                               row.RelativeItem().Element( container =>
                               {
                                   container.Table( table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(6);
                                           columns.RelativeColumn(2);
                                       });     
                                        table.Cell().Image("Assets/logo_normal.png");
                                        table.Cell().Text("UNIDAD DE PLANIFICACIÓN").AlignCenter().SemiBold();
                                        table.Cell().Text($"N° PV/CP/{certificacion.ce.correlativo.ToString("00000")}/2025");
                                   });
                               });
                            });
                        column.Item().PaddingVertical(10).AlignCenter().Text("CERTIFICACIÓN POA").SemiBold().FontSize(12).FontColor(Colors.Black);
                    });
                    page.Content().Element(container =>
                    {
                        container.Column(column =>
                        {
                            column.Item().Row( row =>
                            {
                                row.RelativeItem().Element(container =>
                                {
                                   container.Table(table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(5);
                                       });     
                                        table.Cell().Border(1).Text("UNIDAD ORGANIZACIONAL:").SemiBold();
                                        table.Cell().BorderBottom(1).Text(empleadoSol?.d?.descripcion);
                                        table.Cell().Text("").SemiBold();
                                        table.Cell().Text("");
                                        table.Cell().Border(1).Text("UNIDAD/ÁREA SOLICITANTE").SemiBold();
                                        table.Cell().BorderBottom(1).Text(empleadoSol?.d?.descripcion);
                                   });
                                });
                            });
                            column.Item().Row( row =>
                            {
                                row.RelativeItem().Element(container =>
                                {
                                   container.Table(table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.RelativeColumn(3);
                                           columns.RelativeColumn(3);
                                       });     
                                        table.Cell().Border(1).Text("SOLICITADO POR:").SemiBold();
                                        table.Cell().Border(1).Text("INMEDIATO SUPERIOR").SemiBold();
                                        table.Cell().BorderBottom(1).Text($"Nombre y Apellidos: {empleadoSol.p.Nombres} {empleadoSol.p.apellido_paterno} {empleadoSol.p.apellido_materno}");
                                        table.Cell().BorderBottom(1).Text($"Nombre y Apellidos: {empleadoDesignacion.p.Nombres} {empleadoDesignacion.p.apellido_paterno} {empleadoDesignacion.p.apellido_materno}");
                                        table.Cell().BorderBottom(1).Text($"Cargo: {empleadoSol.e.DenominacionCargo}");
                                        table.Cell().BorderBottom(1).Text($"Cargo: {empleadoDesignacion.e.DenominacionCargo}");
                                   });
                                });
                            });
                            column.Item()
                                .PaddingVertical(5)
                                .Text("La Unidad de Planificación dependiente de la Dirección General Ejecutiva, dertifica: que los bienes, obras o servicios descritos son concordantes con las acciones mediano plazo, corto plazo y las operaciones establecidas en el Plan Operativo Anual (POA) de la Agencia de Infraestructura en Salud y Equipamiento Médico, para la presente gestión.")
                                .Justify();
                            column.Item()
                                .PaddingVertical(5)
                                .Text("1. DETALLE DE LA CERTIFICACIÓN SOLICITADA (BIEN, OBRA O SERVICIO)")
                                .Justify();
                            column.Item().
                                PaddingVertical(20)
                                .Border(1)
                                .Text("CERTIFICACION POA PARA PAGO DE VIÁTICOS")
                                .Justify();
                            
                            column.Item()
                                .PaddingVertical(5)
                                .Text("Nota. para el caso de procesos inscritos en el PAC, se registra el detalle de acuerdo a la referencia del informe y/o  nota de la Unidad Solicitante.")
                                .Justify();
                            
                            column.Item()
                                .PaddingVertical(5)
                                .Text("2. DESCRIPCIÓN DE LA CERTIFICACIÓN POA")
                                .Justify();
                            column.Item().Row( row =>
                            {
                                row.RelativeItem().Element(container =>
                                {
                                   container.Table(table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(5);
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(4);
                                       });     
                                        table.Cell().Border(1).Text("COD. AMP").SemiBold();
                                        table.Cell().Border(1).Text("DESCRIPCIÓN AMP").SemiBold();
                                        table.Cell().Border(1).Text("COD. ACP").SemiBold();
                                        table.Cell().Border(1).Text("DESCRIPCIÓN ACP").SemiBold();
                                        table.Cell().Border(1).Text("6.6.1.3").AlignCenter();
                                        table.Cell().Border(1).Text("CONSTRUIR Y EQUIPAR 18 ESTABLECIMIENTOS DE SALUD DE TERCER NIVEL");
                                        table.Cell().Border(1).Text("ACP7").AlignCenter();
                                        table.Cell().Border(1).Text("FORTALECER EL 100% EL DESARROLLO DE LAS INFRAESTRUCTURAS EN SALUD CON APOYO DE RECURSOS HUMANOS Y TÉCNICOS EN EL TERRITORIO NACIONAL POR LA AISEM EN LA GESTION 2025");
                                   });
                                });
                            });
                            column.Item().Row( row =>
                            {
                                row.RelativeItem().Element(container =>
                                {
                                   container.Table(table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(5);
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(3);
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(2);
                                       });     
                                        table.Cell().Border(1).Text("COD. OP.").SemiBold().AlignCenter();
                                        table.Cell().Border(1).Text("NOMBRE OPERACIÓN").SemiBold().AlignCenter();
                                        table.Cell().Border(1).Text("PROG.").SemiBold().AlignCenter();
                                        table.Cell().Border(1).Text("PROY.").SemiBold().AlignCenter();
                                        table.Cell().Border(1).Text("ACT.").SemiBold().AlignCenter();
                                        table.Cell().Border(1).Text("FUENTE").SemiBold().AlignCenter();

                                        table.Cell().Border(1).Text("OP. 40").AlignCenter();
                                        table.Cell().Border(1).Text("FORTAL. PLAN DE HOSPITALES A NIVEL NACIONAL").AlignCenter();
                                        table.Cell().Border(1).Text("720").AlignCenter();
                                        table.Cell().Border(1).Text(" 3820004600000").AlignCenter();
                                        table.Cell().Border(1).Text("0").AlignCenter();
                                        table.Cell().Border(1).Text("41-111").AlignCenter();
                                   });
                                });
                            });
                            column.Item().Row( row =>
                            {
                                row.RelativeItem().Element(container =>
                                {
                                   container.Table(table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(2);

                                       });     
                                        table.Cell().Border(1).Height(170).Text("Elaborado Por:").SemiBold().AlignCenter();
                                        table.Cell().Border(1).Text("Aprobado Por:").SemiBold().AlignCenter();
                                        table.Cell().Border(1).Text($"Documento de Referencia: {solicitud.cite_memo}");
                                   });
                                });
                            });
                        });
                    });
                });
            });
            return document.GeneratePdf();
        }

        public async Task<Byte[]> getCertificacionPresupuestaria(int solicitudId)
        {
            var solicitud = await (from s in db.solicitudViajes
                            join f in db.fuenteFinanciamientos on s.fuenteId equals f.Id
                            where solicitudId == s.Id
                            select new
                            {
                                s.cite_memo,
                                s.createdAt,
                                s.empleadoId,
                                descripcion_fuente = f.descripcion,
                                f.codigo_fuente,
                                literal_fuente = f.literal,
                                s.fechaInicio,
                                s.fechaFin,
                                s.descripcion_viaje
                            }).FirstOrDefaultAsync();
            var empleadoSol = await (from e in userDb.EmpleadosContratos
                                    join p in userDb.Personas on e.personaId equals p.Id
                                    where solicitud.empleadoId == e.Id
                                    select new
                                    {
                                        e, p
                                    }).FirstOrDefaultAsync();
            var certificacion = await (from cp in db.certificacionPresupuestarias
                                        join cat in db.categoriasProgramaticas on cp.categoriaId equals cat.Id
                                        where solicitudId == cp.solicitudId
                                        select new
                                        {
                                        cp.concepto,
                                        cp.correlativo,
                                        cp.createdAt,
                                        cp.importe_solicitado,
                                        cp.saldo_categoria,
                                        cat.descripcion_categoria,
                                        cat.categoria,
                                        cat.partida,
                                        cat.presupuesto_vigente
                                        }).FirstOrDefaultAsync();
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter);
                    page.MarginHorizontal(50f);
                    page.MarginVertical(0f);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(9));

                     page.Header().Column(column =>
                    {
                        column.Item().Row( row =>
                            {
                               row.RelativeItem().Element( container =>
                               {
                                   container.Table( table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(6);
                                           columns.RelativeColumn(2);
                                       });     
                                        table.Cell().Image("Assets/logo_normal.png");
                                        table.Cell().Text("UNIDAD FINANCIERA \n Área de Presupuesto").AlignCenter().SemiBold();
                                        table.Cell().Text($"N° PV/PS/{certificacion.correlativo.ToString("00000")}/2025");
                                   });
                               });
                            });
                        column.Item().PaddingVertical(10).AlignCenter().Text("CERTIFICACIÓN PRESUPUESTARIA").SemiBold().FontSize(12).FontColor(Colors.Black);
                    });
                    page.Content().Element(container =>
                    {
                        container.Column(column =>
                        {
                            column.Item()
                                .PaddingVertical(5)
                                .Text("De acuerdo a la aprobación del Presupuesto General del Estado 2025, mediante LeyN° 1613, el cual incluye el Presupuesto de la Agencia de la Infraestructura en Salud y Equipamiento Médico, el Área de Presupuesto de la Unidad Financiera.")
                                .Justify();
                            column.Item()
                                .PaddingVertical(5)
                                .Text("CERTIFICA:")
                                .Justify();
                            column.Item()
                                .PaddingVertical(5)
                                .Text($"Que el Presupuesto para la Gestion {DateTime.UtcNow.Year}, contempla la programación presupuestaria, según la siguiente Estructura Programática:")
                                .Justify();
                            column.Item().Row( row =>
                            {
                               row.RelativeItem().Element( container =>
                               {
                                   container.Table( table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(0.5f);
                                           columns.RelativeColumn(3);
                                       });     
                                        table.Cell().Text("Entidad");
                                        table.Cell().Text("382");
                                        table.Cell().Text("Agencia de Infraestructura en Salud y Equipamiento Médico");
                                        table.Cell().Text("Dir. Adm.:");
                                        table.Cell().Text("01");
                                        table.Cell().Text("DIR. GRAL. AENCIA DE INFRAESTRUCTURA EN SALUD Y EQUIPAMIENTO MEDICO - AISEM");
                                        table.Cell().Text("Unidad Ejecutora:");
                                        table.Cell().Text("001");
                                        table.Cell().Text("DIRECCION DE ADMINISTRACION Y FINANZAS");
                                        table.Cell().Text("Fuente:");
                                        table.Cell().Text(solicitud.codigo_fuente.ToString());
                                        table.Cell().Text($"TRANSFERENCIAS {solicitud.descripcion_fuente}");
                                        table.Cell().Text("Organismo:");
                                        table.Cell().Text("111");
                                        table.Cell().Text(solicitud.literal_fuente);
                                        table.Cell().Text("Descripción Categoria Programatica:");
                                        table.Cell().Text("");
                                        table.Cell().Text(certificacion.descripcion_categoria);
                                        table.Cell().Text("Concepto:");
                                        table.Cell().ColumnSpan(2).Text(certificacion.concepto);
                                   });
                               });
                            });
                            column.Item().PaddingVertical(5).Row( row =>
                            {
                               row.RelativeItem().Element( container =>
                               {
                                   container.Table( table =>
                                   {
                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(4);
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(3);
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(2);
                                           columns.RelativeColumn(2);
                                       });     
                                        table.Cell().RowSpan(2).Border(1).Text("Prog").SemiBold();
                                        table.Cell().RowSpan(2).Border(1).Text("Categoria Programatica").SemiBold();
                                        table.Cell().RowSpan(2).Border(1).Text("Partida").SemiBold();
                                        table.Cell().RowSpan(2).Border(1).Text("Descripción").SemiBold();
                                        table.Cell().Border(1).Text("Presupuesto Vigente").FontSize(8).SemiBold();
                                        table.Cell().Border(1).Text("Certificación Acomulada").FontSize(8).SemiBold();
                                        table.Cell().Border(1).Text("Importe Solicitado").SemiBold();
                                        table.Cell().Border(1).Text("Saldo").SemiBold();
                                        table.Cell().Border(1).Text("Saldo").SemiBold();
                                        table.Cell().Border(1).Text("Bs.").SemiBold().AlignCenter();
                                        table.Cell().Border(1).Text("Bs.").SemiBold().AlignCenter();
                                        table.Cell().Border(1).Text("Bs.").SemiBold().AlignCenter();
                                        table.Cell().Border(1).Text("Bs.").SemiBold().AlignCenter();
                                        table.Cell().Border(1).Text("Bs.").SemiBold().AlignCenter();
                                        // Replace data
                                        table.Cell().Border(1).Text("720").AlignCenter();
                                        table.Cell().Border(1).Text(certificacion.categoria).AlignCenter();
                                        table.Cell().Border(1).Text(certificacion.partida.ToString()).AlignCenter();
                                        table.Cell().Border(1).Text("Viaticos por Viajes al interior del País").AlignCenter();
                                        table.Cell().Border(1).Text(certificacion.presupuesto_vigente.ToString("0.00")).AlignCenter();
                                        table.Cell().Border(1).Text((certificacion.presupuesto_vigente - certificacion.saldo_categoria - certificacion.importe_solicitado).ToString("0.00")).AlignCenter();
                                        table.Cell().Border(1).Text(certificacion.importe_solicitado.ToString("0.00")).AlignCenter();
                                        table.Cell().Border(1).Text(certificacion.saldo_categoria.ToString("0.00")).AlignCenter();
                                        table.Cell().Border(1).Text("SI").AlignCenter();
                                        table.Cell().ColumnSpan(4).Border(1).Text("TOTAL Bs.").AlignRight();
                                        table.Cell().Border(1).Text(certificacion.presupuesto_vigente.ToString("0.00")).AlignCenter();
                                        table.Cell().Border(1).Text((certificacion.presupuesto_vigente - certificacion.saldo_categoria - certificacion.importe_solicitado).ToString("0.00")).AlignCenter();
                                        table.Cell().Border(1).Text(certificacion.importe_solicitado.ToString("0.00")).AlignCenter();
                                        table.Cell().Border(1).Text(certificacion.saldo_categoria.ToString("0.00")).AlignCenter();
                                        table.Cell().Border(1).Text("").AlignCenter();
                                   });
                               });
                            });
                            column.Item()
                                .PaddingVertical(5)
                                .Text("Es cuanto se certifica para fines consiguientes.")
                                .Justify();
                            column.Item()
                                .PaddingVertical(20)
                                .AlignCenter()
                                .Text($"La Paz, {certificacion.createdAt.ToLongDateString()}");
                        });
                    });
                });
            });
            return document.GeneratePdf();
        }

    }
}