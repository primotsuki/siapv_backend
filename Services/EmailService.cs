using siapv_backend.DB;
using siapv_backend.Models;
using FluentEmail.Core;
using Microsoft.EntityFrameworkCore;
namespace siapv_backend.Services
{
    public class EmailService: IEmailService
    {
        private readonly IFluentEmail _fluentEmail;
        public readonly AppDbContext db;
        public readonly DBUsuariosContext usrDb;
        public EmailService (IFluentEmail fluentEmail, AppDbContext _db, DBUsuariosContext _usrDb)
        {
            _fluentEmail = fluentEmail;
            db = _db;
            usrDb = _usrDb;
        }
        public async Task<Boolean> sendInicioViaje(SolicitudViaje solicitud)
        {
            // buscar encargados de pasajes
            var encargadosRoles = db.userRoles.Where(x=> x.roleId == 2).ToList();
            var empleados = (from p in usrDb.Personas 
                                join us in usrDb.Usuarios on p.Id equals us.PersonaId
                                join em in usrDb.EmpleadosContratos on p.Id equals em.personaId
                                where em.activo select new
                                {
                                    correo = us.email,
                                    userId = us.Id
                                }).ToList();
            var encargados = (from e in encargadosRoles
                    join em in empleados on e.userId equals em.userId
                    select new
                    {
                        em.correo
                    }).ToList();
            // buscar designador
            var designador = (from p in usrDb.Personas 
                                join us in usrDb.Usuarios on p.Id equals us.PersonaId
                                join em in usrDb.EmpleadosContratos on p.Id equals em.personaId
                                where em.Id == solicitud.designadorId
                                select new
                                {
                                    correo = us.email
                                }).FirstOrDefaultAsync();
            // buscar designado
            var designado = await (from p in usrDb.Personas 
                                join us in usrDb.Usuarios on p.Id equals us.PersonaId
                                join em in usrDb.EmpleadosContratos on p.Id equals em.personaId
                                where em.Id == solicitud.empleadoId
                                select new
                                {
                                    correo = us.email,
                                    nombres = p.Nombres,
                                    p.apellido_paterno,
                                    p.apellido_materno
                                }).FirstOrDefaultAsync();
            var destino = await db.lugarDestinos.FirstOrDefaultAsync(x=>x.Id == solicitud.lugarDestinoId);;
            var email = await _fluentEmail
            .To(encargados[0].correo)
            .Subject("Designacion de Viaje")
            .UsingTemplateFromFile("Templates/comision_viaje.cshtml", new
            {
                title = "Designacion de Viaje",
                nombre_designado = designado?.nombres + " " + designado?.apellido_paterno + " " + designado?.apellido_materno,
                destino?.destino,
                fecha_inicio = solicitud.fechaInicio.ToLongDateString(),
                fecha_fin = solicitud.fechaFin.ToLongDateString(),
                descripcion = solicitud.descripcion_viaje,
                mensaje_email = "Favor de realizar las acciones correspondientes de acuerdo a normativa vigente, compra de pasajes aereos (si corresponde).",
                Url="http://siapv.aisem.gob.bo",
                Text="Ver Designación"
            }).SendAsync();
            return true;
        }
    }
}